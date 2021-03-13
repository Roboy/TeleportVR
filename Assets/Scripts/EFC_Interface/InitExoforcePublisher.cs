using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using UnityEngine.InputSystem;
using Pose = RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;

public class InitExoforcePublisher : RosPublisher<InitExoforceRequest>
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public void InitExoforce()
    {
        if (!CageInterface.cageIsConnected)
        {
            // send init message
            string[] ef_name = {"left_hand", "right_hand"}; // posible values ('left_hand'/'right_hand')
            bool[] ef_enabled = {true, true};
            Pose[] poses = {CageInterface.TransformToPose(leftHand), CageInterface.TransformToPose(rightHand)};

            // read out the head position. If it isnt tracked, send 1.7 as a mock
            float headHeight = head.position.y;
            if (headHeight == 0)
            {
                headHeight = 1.7f;
            }
                    
            InitExoforceRequest msg =
                new InitExoforceRequest(ef_name, ef_enabled, poses, headHeight);
            PublishMessage(msg);
            CageInterface.OnInitRequest();
        }
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && !CageInterface.sentInitRequest)
        {
            InitExoforce();
        }

        if (InputManager.Instance.GetControllerBtn(UnityEngine.XR.CommonUsages.gripButton, true) &&
            InputManager.Instance.GetControllerBtn(UnityEngine.XR.CommonUsages.gripButton, false) &&
            !CageInterface.sentInitRequest)
        {
            InitExoforce();
        }
    }
}
