using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace RudderPedals
{
    public class PedalDriver : Singleton<PedalDriver>
    {
        public class MovingAverageFilter
        {
            private Queue<float> buffer;
            private int filterSize;
            public MovingAverageFilter(int filterSize)
            {
                buffer = new Queue<float>(filterSize);
                this.filterSize = filterSize;
            }

            public void Add(float value)
            {
                buffer.Enqueue(value);
                if (buffer.Count >= filterSize)
                    buffer.Dequeue();
            }

            public float GetFiltered()
            {
                double sum = 0;
                foreach (var item in buffer)
                {
                    sum += item;
                }
                return (float)(sum / buffer.Count);
            }

            public void Reset()
            {
                buffer.Clear();
            }
        }


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
        public int filterSize = 10;

        [Header("Read Only values")]
        public float velocity;
        public float angularVelocity;
        public Vector2 output
        {
            get { return _output; }
        }

        [SerializeField] private Vector2 _output;
        public Vector2 normalizedOutput
        {
            get { return _output / maxCompMag; }
        }
        private float maxCompMag;

        private const int playerId = 0;
        private readonly Vector2 leftDrive = new Vector2(-1f, 1f);
        private readonly Vector2 rightDrive = new Vector2(1f, -1f);
        private readonly Vector2 forwardDrive = new Vector2(1f, 1f);
        private Player player;
        [SerializeField] private bool goingForward = true;
        [SerializeField] private bool canChangeDir = true;

        private MovingAverageFilter leftFilter, rightFilter;


        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);
            velocityMap.preWrapMode = WrapMode.Clamp;
            velocityMap.postWrapMode = WrapMode.Clamp;
            angularVelocityMap.preWrapMode = WrapMode.Clamp;
            angularVelocityMap.postWrapMode = WrapMode.Clamp;

            leftFilter = new MovingAverageFilter(filterSize);
            rightFilter = new MovingAverageFilter(filterSize);
            // maximal magnitude of any output component (projected length of longest output vector on any reference axis)
            maxCompMag = (maxVelocity + maxAngularVelocity) / Mathf.Sqrt(2);
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

            leftFilter.Add(left);
            rightFilter.Add(right);
            left = leftFilter.GetFiltered();
            right = rightFilter.GetFiltered();

            // go forward if one pedal is pressed more than the forward deadzone
            bool isForward = Mathf.Max(left, right) >= forwardDeadzone;
            // go back if both pedals are pressed in more than the backward deadzone
            bool isBackward = Mathf.Min(left, right) >= backwardDeadzone;


            // only change direction if pedals are in both deadzones
            if (Mathf.Max(left, right) <= Mathf.Max(forwardDeadzone, backwardDeadzone))
            {
                canChangeDir = true;
            }

            if (canChangeDir)
            {
                if (isBackward)
                {
                    goingForward = false;
                    canChangeDir = false;
                    leftFilter.Reset();
                    rightFilter.Reset();
                }
                else if (isForward)
                {
                    goingForward = true;
                    canChangeDir = false;
                    leftFilter.Reset();
                    rightFilter.Reset();
                }
            }

            float vel = Mathf.Max(left, right);
            // in [-1, 1]
            float mappedAngularVelocity = Mathf.Sign(steeringAngle) * Mathf.Clamp01(angularVelocityMap.Evaluate(Mathf.Abs(steeringAngle)));
            // in [-1, 1]
            float mappedVelocity;
            if (goingForward)
            {
                vel = Mathf.Clamp01(vel - forwardDeadzone) / (1 - forwardDeadzone);
                mappedVelocity = Mathf.Clamp01(velocityMap.Evaluate(Mathf.Abs(vel)));
            }
            else
            {
                vel = Mathf.Clamp01(vel - backwardDeadzone) / (1 - backwardDeadzone);
                mappedVelocity = -0.5f * Mathf.Clamp01(velocityMap.Evaluate(Mathf.Abs(vel)));
            }


            Vector2 direction = Vector2.Lerp(leftDrive.normalized,
                                             rightDrive.normalized,
                                             0.5f + 0.5f * mappedAngularVelocity);

            // publish read only values
            velocity = mappedVelocity * maxVelocity;
            angularVelocity = direction.magnitude * maxAngularVelocity;

            _output = maxAngularVelocity * direction + velocity * forwardDrive.normalized;

            driveControl.V_L = output.x;
            driveControl.V_R = output.y;
        }

    }

}

