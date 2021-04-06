using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI trackingStateText;
    public TextMeshProUGUI loggerText;
    public bool useTracking = true;
    public RawImage[] connectionStates;

    private static string log = "";
    private static bool newMessage = false;

    private static bool animusConnected = false;
    private static bool clientConnected = false;
    private static bool sharedMemoryConnected = false;

    private static bool newConnection = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (newConnection)
        {
            UpdateConnectionStates();
            newConnection = false;
        }
        
        if (newMessage)
        {
            loggerText.text = log;
            newMessage = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            useTracking = !useTracking;

            if (useTracking)
            {
                UnityAnimusClient.sendTransformToCam = true;
                FlyCam.sendTransformToCamera = false;

                trackingStateText.text = "Tracking";
            }
            else
            {
                UnityAnimusClient.sendTransformToCam = false;
                FlyCam.sendTransformToCamera = true;
                
                trackingStateText.text = "Free View";
            }
        }
    }

    public static void WriteToLogger(string logMessage)
    {
        log = log + "\n\n[" + 
              System.DateTime.Now.ToString("HH:mm:ss") + "]\n" +
              logMessage;
        newMessage = true;
    }

    private void UpdateConnectionStates()
    {
        if (animusConnected)
        {
            connectionStates[0].color = Color.green;
        }
        else
        {
            connectionStates[0].color = Color.red;
        }
        
        if (clientConnected)
        {
            connectionStates[1].color = Color.green;
        }
        else
        {
            connectionStates[1].color = Color.red;
        }
        
        if (sharedMemoryConnected)
        {
            connectionStates[2].color = Color.green;
        }
        else
        {
            connectionStates[2].color = Color.red;
        }
    }

    public static void SetConnectionState(int id, bool connected)
    {
        newConnection = true;
        
        switch (id)
        {
            case 0:
                animusConnected = connected;
                break;
            
            case 1:
                clientConnected = connected;
                break;
            
            case 2:
                sharedMemoryConnected = connected;
                break;
        }
    }
}
