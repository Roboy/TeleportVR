#if ROSSHARP
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

    /// <summary>
    /// Converts a Transform and given name to an EFPose
    /// </summary>
    /// <param name="ef_transform">The transform of which the data should be read out.</param>
    /// <param name="hand_id">The name of the transform/effector.</param>
    /// <returns></returns>
    private EFPose transformToEFPose(Transform ef_transform, string hand_id)
    {
        Pose pose = CageInterface.TransformToPose(ef_transform);
        return new EFPose(hand_id, pose);
    }

    /// <summary>
    /// Publishes the current hand poses to the cage every frame, if connected.
    /// </summary>
    void Update()
    {
        if (CageInterface.cageIsConnected)
        {
            PublishMessage(transformToEFPose(handLeft, "left_hand"));
            PublishMessage(transformToEFPose(handRight, "right_hand"));
        }
    }
}
#endif
