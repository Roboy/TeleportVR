using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp;
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
public class CageInterface : Singleton<CageInterface>
{
    [SerializeField] private CollisionPublisher collisionPublisher;
    [SerializeField] private CloseCagePublisher closeCagePublisher;
    
    public static bool cageIsConnected;
    public static bool sentInitRequest;

    private static float connectionTimeout = 5f;
    private static float _connectionTimer;

    public static Pose TransformToPose(Transform t)
    {
        Quaternion q = t.rotation;
        q = q.Unity2Ros();
        RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion rot =
            new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion(q.x, q.y, q.z, q.w);
        Vector3 p = t.position;
        p = p.Unity2Ros();
        return new Pose(new Point(p.x, p.y, p.z), rot);
    }

    private void Awake()
    {
        // Look for the instance of the Cage in the correct scene and save it
        _ = Instance;
    }

    private void Update()
    {
        if (_connectionTimer <= 0)
        {
            sentInitRequest = false;
        }
        else
        {
            _connectionTimer -= Time.deltaTime;
        }
    }

    public static void OnInitRequest()
    {
        sentInitRequest = true;
        _connectionTimer = connectionTimeout;
    }

    public void ForwardCollisions(float[] collisionData)
    {
        collisionPublisher.PublishCollision(collisionData);
    }

    public void CloseCage()
    {
        if (cageIsConnected)
        {
            closeCagePublisher.Publish();
        }
        else
        {
            Debug.Log("Tried to close not initialized cage.");
        }
    }
}
