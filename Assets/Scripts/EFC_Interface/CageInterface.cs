using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;

//using RosSharp.RosBridgeClient.MessageTypes.Geometry;

[RequireComponent(typeof(RosConnector))]
public class CageInterface : MonoBehaviour
{
    [SerializeField] private string topicInit;
    [SerializeField] private string topicClose;
    
    public static bool cageIsConnected;
    
    private RosConnector _cageRosConnector;
    private string initPublicationId;
    private string initResponsePublicationId;
    private string closePublicationId;

    private bool started;
    
    // Start is called before the first frame update
    void Start()
    {
        _cageRosConnector = GetComponent<RosConnector>();
        StartCoroutine(StartPublisher(1.0f));
    }
    
    /// <summary>
    ///  Start method of InterfaceMessage.
    /// Starts a coroutine to initialize the publisher after 1 second to prevent race conditions.
    /// </summary>
    /*protected override void Start()
    {
        StartCoroutine(StartPublisher(1.0f));
    }*/
    /// <summary>
    /// Starts the publisher.
    /// </summary>
    /// <returns>The publisher.</returns>
    /// <param name="waitTime">Wait time.</param>
    private IEnumerator StartPublisher(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            initPublicationId = _cageRosConnector.RosSocket.Advertise<InitExoforceRequest>(topicInit);
            closePublicationId = _cageRosConnector.RosSocket.Advertise<RosSharp.RosBridgeClient.MessageTypes.Std.Empty>(topicClose);
            started = true;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //InitExoforceResponse response = new InitExoforceResponse();
                //_cageRosConnector.RosSocket.Publish(initResponsePublicationId, response);
                //print("Send a response");
                
                if (cageIsConnected)
                {
                    // Send disconnect message
                    RosSharp.RosBridgeClient.MessageTypes.Std.Empty msg =
                        new RosSharp.RosBridgeClient.MessageTypes.Std.Empty();
                    _cageRosConnector.RosSocket.Publish(closePublicationId, msg);
                    cageIsConnected = false;
                }
                else
                {
                    // send init message
                    string[] ef_name = {"left_hand"}; // posible values ('left_hand'/'right_hand')
                    bool[] ef_enabled = {true};
                    RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion rot =
                        new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion();
                    RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose[] ef_init_pose = {new RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose(new Point(0, 0, 0), rot) };
                    
                    RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 calibration_point1 = 
                        new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0, 0, 0);
                    RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 calibration_point2 = 
                        new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0, 0, 0);
                    RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3 calibration_point3 = 
                        new RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3(0, 0, 0);
                    RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3[] calibration_points =
                        {calibration_point1, calibration_point2, calibration_point3};

                    InitExoforceRequest msg =
                        new InitExoforceRequest(ef_name, ef_enabled, ef_init_pose, calibration_points);
                    print("Publishing " + msg + " on " + initPublicationId);
                    _cageRosConnector.RosSocket.Publish(initPublicationId, msg);
                    cageIsConnected = true;
                }
            }
        }
        else
        {
            Debug.LogWarning("Couldn't send message to the cage, publisher has not been started");
        }
    }
}
