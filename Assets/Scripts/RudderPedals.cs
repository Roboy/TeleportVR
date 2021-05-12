using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


[RequireComponent(typeof(CharacterController))]
public class RudderPedals : MonoBehaviour
{

    public int playerId = 0;
    public DifferentialDriveControl driveControl;

    private Player player;
    //private CharacterController cc;

    public float maxForward = 1f;
    public float maxBackward = 1f;
    public float maxSpeed = 0.3f;
    // forward steering [0,1]
    // backward steering [0,1]

    public Vector2 left = new Vector2(0.1f, 0.2f);
    public Vector2 right = new Vector2(0.2f, 0.1f);
    public Vector2 drive;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        //cc = GetComponent<CharacterController>();
    }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // remap to [0,1]
        float steeringAngle = 0.5f + 0.5f * player.GetAxis("SteeringAngle");
        float forward = player.GetAxis("Forward");
        float backward = player.GetAxis("Backward");

        forward = Mathf.Max(forward, backward);
        backward = 0;

        drive = Vector2.Lerp(left, right, steeringAngle);
        drive /= drive.magnitude;
        drive *= maxSpeed;
        drive = (-backward + forward) * drive;

        driveControl.V_L = drive.x;
        driveControl.V_R = drive.y;
    }
}
