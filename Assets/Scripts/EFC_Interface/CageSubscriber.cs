using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using UnityEngine;

public class CageSubscriber : UnitySubscriber<ExoforceResponse>
{

    protected override void ReceiveMessage(ExoforceResponse message)
    {
        Debug.Log(message.success + ": Received " + message.message);
        if (message.success)
        {
            CageInterface.cageIsConnected = true;
        }
    }
}
