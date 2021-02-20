using System.Collections;
using System.Collections.Generic;
using Animus.RobotProto;
using AnimusClient;
using AnimusManager;
using UnityEngine;

public class ClientLogic : MonoBehaviour
{
    public AnimusClientManager AnimusManager;
    public UnityAnimusClient unityClient;
    
    public string robotName;
    public string AccountEmail;
    public string AccountPassword;
    
    private Robot _chosenRobot;
    private int _count;
    
    // Start is called before the first frame update
    void Start()
    {
        _count = 0;
        StartCoroutine(ClientManagerLogic());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ClientManagerLogic()
    {
        // Yield for 10 frames to allow other scripts to initialise first
        if (_count < 10)
        {
            _count++;
            yield return null; 
        }

        // Step 1 - Login user
        AnimusManager.LoginUser(AccountEmail, AccountPassword);
        while (!AnimusManager.loginResultAvailable)
        {
            yield return null;
        }
        if (!AnimusManager.loginSuccess) yield break;
        Debug.Log("Login successful.");
        
        // Step 2 - Search for connectable robots
        AnimusManager.SearchRobots();
        while (!AnimusManager.searchResultsAvailable)
        {
            yield return null;
        }
        if (!AnimusManager.searchSuccess) yield break;
    
        // Step 3 - Choose Robot
        foreach (var robot in AnimusManager.robotDetailsList)
        {
            Debug.Log(robot.ToString());
            if (robot.Name == robotName)
            {
                _chosenRobot = robot;
            }
        }
        
        if (_chosenRobot == null)
        {
            Debug.Log($"Robot {robotName} not found");
            yield break;
        }
        
        Debug.Log($"Found robot {robotName}");
             
        //Step 4 - Send AnimusManager the interface it should use for this connection
        AnimusManager.SetClientClass(unityClient);
        
        // Step 5 - Connect to the robot
        AnimusManager.StartRobotConnection(_chosenRobot);
        while (!AnimusManager.connectToRobotFinished)
        {
            yield return null;
        }
        if (!AnimusManager.connectedToRobotSuccess) yield break;
        
        // Step 5 - Starting all modalities
        // var requiredModalities = new string[] {"vision", "audition", "voice" };
        var requiredModalities = new string[] {"vision"};
        AnimusManager.OpenModalities(requiredModalities);
    }
}
