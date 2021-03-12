using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using Pose = RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;

public class InitExoforcePublisher : RosPublisher<InitExoforceRequest>
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!CageInterface.cageIsConnected)
            {
                // send init message
                string[] ef_name = {"left_hand", "right_hand"}; // posible values ('left_hand'/'right_hand')
                bool[] ef_enabled = {true, true};
                Pose[] poses = {CageInterface.transformToPose(leftHand), CageInterface.transformToPose(rightHand)};

                // read out the head position. If it isnt tracked, send 1.7 as a mock
                float headHeight = head.position.y;
                if (headHeight == 0)
                {
                    headHeight = 1.7f;
                }
                    
                InitExoforceRequest msg =
                    new InitExoforceRequest(ef_name, ef_enabled, poses, headHeight);
                PublishMessage(msg);
            }
        }
    }
}
