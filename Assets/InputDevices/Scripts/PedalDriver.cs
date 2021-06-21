using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace RudderPedals
{
    public class PedalDriver : Singleton<PedalDriver>
    {
        public new bool enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                // set velocity to 0 when not enabled
                if (!_enabled)
                {
                    _output = Vector2.zero;
                    driveControl.V_L = _output.x;
                    driveControl.V_R = _output.y;

                    ResetMovingAverageFilter();
                }
            }
        }
        private bool _enabled = true;

        public DifferentialDriveControl driveControl;

        [Range(0, 1), Tooltip("Maximum forward velocity (m/s)")]
        public float maxVelocity = 0.1f;

        [Range(0, 1), Tooltip("Maximum angular velocity (m/s)")]
        public float maxAngularVelocity = 0.025f;

        [Range(0, 1), Tooltip("Foot bedal deadzone for going forwards")]
        public float forwardDeadzone = 0.4f;
        [Range(0, 1), Tooltip("Foot pedal deadzone for going backwards")]
        public float backwardDeadzone = 0.2f;

        public AnimationCurve velocityMap;
        public AnimationCurve angularVelocityMap;

        [Tooltip("Number of steps to look back for smoothing wheelchair velocity. Only set at startup")]
        public int velocityFilterSize = 10;
        private Queue<float> movingAverageCache;

        [Header("Read Only values")]
        public float velocity;
        public float angularVelocity;

        public Vector2 output
        {
            get { return _output; }
        }

        private Vector2 _output;

        private const int playerId = 0;
        private readonly Vector2 leftDrive = new Vector2(-1f, 1f);
        private readonly Vector2 rightDrive = new Vector2(1f, -1f);
        private readonly Vector2 forward = new Vector2(1f, 1f);
        private Player player;


        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);
            velocityMap.preWrapMode = WrapMode.Clamp;
            velocityMap.postWrapMode = WrapMode.Clamp;
            angularVelocityMap.preWrapMode = WrapMode.Clamp;
            angularVelocityMap.postWrapMode = WrapMode.Clamp;

            movingAverageCache = new Queue<float>(velocityFilterSize);
            ResetMovingAverageFilter();
        }

        // Update is called once per frame
        void Update()
        {
            if (!enabled)
            {
                return;
            }
            // in  [-1, 1] & inverted
            float steeringAngle = -player.GetAxis("SteeringAngle");
            // in [0,1]
            float left = player.GetAxis("Forward");
            float right = player.GetAxis("Backward");
            float vel = Mathf.Max(left, right);

            if (vel > 0)
            {
                // clip forward to >= deadzone
                // rescale velocity to reach 1 at maximum pedal press
                if (vel <= forwardDeadzone)
                {
                    ResetMovingAverageFilter();
                }
                vel = Mathf.Max(vel - forwardDeadzone, 0) / (1 - forwardDeadzone);
            }
            else
            {
                vel = Mathf.Max(vel - backwardDeadzone, 0) / (1 - backwardDeadzone);
            }

            vel = velocityMap.Evaluate(vel);

            // go back if both pedals are pressed in more than the deadzone
            if (Mathf.Min(left, right) > backwardDeadzone)
            {
                vel = -0.5f * vel;
                ResetMovingAverageFilter();
            }
            else
            {
                vel = MovingAverageFilter(vel);
            }

            Vector2 direction = Vector2.Lerp(leftDrive, rightDrive, 0.5f + 0.5f * angularVelocityMap.Evaluate(Mathf.Abs(steeringAngle)) * Mathf.Sign(steeringAngle));
            velocity = vel * maxVelocity;
            angularVelocity = direction.magnitude * maxAngularVelocity;
            // ||output|| <= max(maxAngularVelocity, maxVelocity)
            _output = maxAngularVelocity * direction + velocity * forward;

            driveControl.V_L = _output.x;
            driveControl.V_R = _output.y;
        }

        /// <summary>
        /// Moving average filtering on wheelchair velocities
        /// </summary>
        /// <param name="values">input value</param>
        /// <returns>smoothed result</returns>
        private float MovingAverageFilter(float values)
        {
            movingAverageCache.Enqueue(values);
            movingAverageCache.Dequeue();
            float sum = 0;
            foreach (var item in movingAverageCache)
            {
                sum += item;
            }
            return sum / movingAverageCache.Count;
        }

        private void ResetMovingAverageFilter()
        {
            movingAverageCache.Clear();
            for (int i = 0; i < velocityFilterSize; i++)
            {
                movingAverageCache.Enqueue(0);
            }

        }
    }

}

