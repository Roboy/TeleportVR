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

//using RosSharp.RosBridgeClient.MessageTypes.Geometry;

[RequireComponent(typeof(RosConnector))]
public class CageInterface : MonoBehaviour
{
    public static bool cageIsConnected;

    public static Pose transformToPose(Transform t)
    {
        Quaternion q = t.rotation;
        RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion rot =
            new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion(q.x, q.y, q.z, q.w);
        Vector3 p = t.position;
        return new Pose(new Point(p.x, p.y, p.z), rot);
    }
}
