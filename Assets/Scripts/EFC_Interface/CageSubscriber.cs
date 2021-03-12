using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using UnityEngine;
using Widgets;

public class CageSubscriber : UnitySubscriber<ExoforceResponse>
{
    [SerializeField] private bool isInit;

    protected override void ReceiveMessage(ExoforceResponse message)
    {
        Debug.Log(message.success + ": Received " + message.message);
        string newIcon = "";
        if (message.success)
        {
            CageInterface.cageIsConnected = isInit;
            if (isInit)
            {
                newIcon = "Cage";
            }
            else
            {
                newIcon = "CageRed";
            }
            // turn the icon to the corresponding icon
            Widget cageWidget = Manager.Instance.FindWidgetWithID(60);
            //var context = cageWidget.GetContext();
            cageWidget.GetContext().currentIcon = newIcon;
            print("context.currentIcon" + cageWidget.GetContext().currentIcon);
            //cageWidget.ProcessRosMessage(cageWidget.GetContext());
        }
        else
        {
            Debug.LogWarning("Request to Cage unssuccessfull: " + message.message);
        }
    }
}
