using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;
using Pose = RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose;
using Quaternion = RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion;
using Transform = UnityEngine.Transform;

public class EFPosePublisher : RosPublisher<EFPose>
{
    [SerializeField] private Transform handLeft;
    [SerializeField] private Transform handRight;

    private EFPose transformToEFPose(Transform ef_transform, string hand_id)
    {
        Pose pose = CageInterface.TransformToPose(ef_transform);
        return new EFPose(hand_id, pose);
    }

    void Update()
    {
        PublishMessage(transformToEFPose(handLeft, "left_hand"));
        PublishMessage(transformToEFPose(handRight, "right_hand"));

    }
}
