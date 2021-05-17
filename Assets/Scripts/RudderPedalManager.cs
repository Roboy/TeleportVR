using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class RudderPedalManager : Singleton<RudderPedalManager>
{

    public DifferentialDriveControl driveControl;

    [Range(0, 1), Tooltip("Maximum forward velocity (m/s)")]
    public float maxVelocity = 0.1f;

    [Range(0, 1), Tooltip("Maximum angular velocity (m/s)")]
    public float maxAngularVelocity = 0.025f;

    [Range(0, 1), Tooltip("Foot bedal deadzone for going forwards")]
    public float forwardDeadzone = 0.4f;
    [Range(0, 1), Tooltip("Foot pedal deadzone for going backwards")]
    public float backwardDeadzone = 0.2f;

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
    }

    // Update is called once per frame
    void Update()
    {
        // remap to [0, 1]
        float steeringAngle = 0.5f + -0.5f * player.GetAxis("SteeringAngle");
        // in [0,1]
        float left = player.GetAxis("Forward");
        float right = player.GetAxis("Backward");
        float vel = Mathf.Max(left, right);
        if (vel > 0)
        {
            // clip forward to >= deadzone
            // rescale velocity to reach 1 at maximum pedal press
            vel = Mathf.Max(vel - forwardDeadzone, 0) / (1 - forwardDeadzone);
        }
        else
        {
            vel = Mathf.Max(vel - backwardDeadzone, 0) / (1 - backwardDeadzone);
        }

        // go back if both pedals are pressed in more than the deadzone
        if (Mathf.Min(left, right) > backwardDeadzone)
        {
            vel = -0.5f * vel;
        }

        // ||output|| <= max(maxAngularVelocity, maxVelocity)
        _output = maxAngularVelocity * Vector2.Lerp(leftDrive, rightDrive, steeringAngle)
            + maxVelocity * vel * forward;

        driveControl.V_L = _output.x;
        driveControl.V_R = _output.y;
    }
}
