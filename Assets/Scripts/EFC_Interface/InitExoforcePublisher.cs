using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using RosSharp.RosBridgeClient.MessageTypes.RoboySimulation;
using UnityEngine.InputSystem;
using Widgets;
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

    private static string[] linkNames = new[]
    {
        "torso",
        "upperarm_right",
        "shoulder_right_link1",
        "shoulder_right_link2",
        "lowerarm_right",
        "elbow_right",
        "hand_right",
        "wrist_right_link1",
        "wrist_right_link2",
        "wrist_right_link",
        "rh_palm",
        "rh_manipulator",
        "rh_ffknuckle",
        "rh_ffproximal",
        "rh_ffmiddle",
        "rh_ffdistal",
        "rh_fftip",
        "rh_mfknuckle",
        "rh_mfproximal",
        "rh_mfmiddle",
        "rh_mfdistal",
        "rh_mftip",
        "rh_rfknuckle",
        "rh_rfproximal",
        "rh_rfmiddle",
        "rh_rfdistal",
        "rh_rftip",
        "rh_lfmetacarpal",
        "rh_lfknuckle",
        "rh_lfproximal",
        "rh_lfmiddle",
        "rh_lfdistal",
        "rh_lftip",
        "rh_thbase",
        "rh_thproximal",
        "rh_thhub",
        "rh_thmiddle",
        "upperarm_left",
        "shoulder_left_link1",
        "shoulder_left_link2",
        "lowerarm_left",
        "elbow_left",
        "hand_left",
        "wrist_left_link1",
        "wrist_left_link2",
        "wrist_left_link",
        "lh_palm",
        "lh_manipulator",
        "lh_ffknuckle",
        "lh_ffproximal",
        "lh_ffmiddle",
        "lh_ffdistal",
        "lh_fftip",
        "lh_mfknuckle",
        "lh_mfproximal",
        "lh_mfmiddle",
        "lh_mfdistal",
        "lh_mftip",
        "lh_rfknuckle",
        "lh_rfproximal",
        "lh_rfmiddle",
        "lh_rfdistal",
        "lh_rftip",
        "lh_lfmetacarpal",
        "lh_lfknuckle",
        "lh_lfproximal",
        "lh_lfmiddle",
        "lh_lfdistal",
        "lh_lftip",
        "lh_thbase",
        "lh_thproximal",
        "lh_thhub",
        "lh_thmiddle",
        "lh_thdistal",
        "lh_thtip",
        "head",
        "head_link2",
        "head_link1",
        "camera_left",
        "camera_right"
    };

    /*private static Dictionary<int, string> linkIdToName = new Dictionary<int, string>()
    {
        {0, "LinkName" },
    };*/

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
            //string linkName = linkIdToName[(int)(info[o+1])];
            string linkName = linkNames[(int)(info[o+1])];
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
        if (!CageInterface.cageIsConnected)
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

            //LinkInformation[] linkInfo = null;
                    
            InitExoforceRequest msg =
                new InitExoforceRequest(ef_name, ef_enabled, poses, headHeight, linkInfo);
            PublishMessage(msg);
            CageInterface.OnInitRequest();
        }
    }

    protected override void OnConnect(bool success)
    {
        // turn the icon to the corresponding icon
        Widget cageWidget = Manager.Instance.FindWidgetWithID(60);
        //var context = cageWidget.GetContext();
        cageWidget.GetContext().currentIcon = "CageRed";
        print("context.currentIcon" + cageWidget.GetContext().currentIcon);
        cageWidget.ProcessRosMessage(cageWidget.GetContext());
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
