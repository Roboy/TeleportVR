#if ROSSHARP
using RosSharp;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using Pose = RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// This is the Class which should handle all interfaces between animus or Teleport and the cage.
/// </summary>
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
        Quaternion q = t.localRotation;
        q = q.Unity2Ros();
        RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion rot =
            new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion(q.x, q.y, q.z, q.w);
        Vector3 p = t.localPosition;
        p = p.Unity2Ros();
        return new Pose(new Point(p.x, p.y, p.z), rot);
    }

    private void Awake()
    {
        // Look for the instance of the Cage in the correct scene and save it
        _ = Instance;
    }

    /// <summary>
    /// Update the timer for the connection
    /// </summary>
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

    // Reset the timer that only allows to send a request every <connectionTimeout> seconds
    public static void OnInitRequest()
    {
        sentInitRequest = true;
        _connectionTimer = connectionTimeout;
    }

    // Delegate the collision Data to the corresponding publisher
    public void ForwardCollisions(float[] collisionData)
    {
        collisionPublisher.PublishCollision(collisionData);
    }

    // Delegate the information to close the cage to the corresponding publisher
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
#endif
