using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using RosSharp.RosBridgeClient.MessageTypes.RoboySimulation;
using UnityEngine.InputSystem;
using Pose = RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;

public class InitExoforcePublisher : RosPublisher<InitExoforceRequest>
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    private static LinkInformation[] linkInfo;
    private static int linkInfoLength = 12;

    private static Dictionary<int, string> linkIdToName = new Dictionary<int, string>()
    {
        {0, "LinkName" },
    };

    public static void StoreLinkInformation(float[] info)
    {
        if (info.Length % linkInfoLength != 0)
        {
            Debug.LogWarning("Shape mismatch: Received array length " + info.Length + " not dividable by " + 
                             linkInfoLength);
        }
        int numLinks = info.Length / linkInfoLength;
        linkInfo = new LinkInformation[numLinks];
        for (int i = 0; i < numLinks; i++)
        {
            int o = i * linkInfoLength;
            int id = (int)(info[o]);
            string linkName = linkIdToName[(int)(info[o+1])];
            RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 dimensions =
                new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(info[o + 2], info[o + 3], info[o + 4]);
            Point point = new Point(info[o + 5], info[o + 6], info[o + 7]);
            RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion quaternion =
                new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion(info[o + 8], info[o + 9], info[o + 10],
                    info[o + 11]);
            Pose init_pose = new Pose(point, quaternion);
            linkInfo[i] = new LinkInformation(id, linkName, dimensions, init_pose);
        }
    }

    public void InitExoforce()
    {
        if (true) //!CageInterface.cageIsConnected)
        {
            // send init message
            string[] ef_name = {"left_hand", "right_hand"}; // posible values ('left_hand'/'right_hand')
            bool[] ef_enabled = {true, true};
            Pose[] poses = {CageInterface.TransformToPose(leftHand), CageInterface.TransformToPose(rightHand)};

            // read out the head position. If it isnt tracked, send 1.7 as a mock
            float headHeight = head.localPosition.y;
            print("Head height: " + headHeight);
            if (headHeight <= 0)
            {
                headHeight = 1.7f;
            }

            LinkInformation[] linkInfo = null;
                    
            InitExoforceRequest msg =
                new InitExoforceRequest(ef_name, ef_enabled, poses, headHeight, linkInfo);
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
