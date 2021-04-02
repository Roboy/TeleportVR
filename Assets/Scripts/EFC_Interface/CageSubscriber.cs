#if ROSSHARP
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware;
using UnityEngine;
using Widgets;

public class CageSubscriber : UnitySubscriber<ExoforceResponse>
{
    [Tooltip("determines if this subscriber is listening to init responses or stop responses")]
    [SerializeField] private bool isInit;

    /// <summary>
    /// Receive and handle simple responses (bool + string) from the cage.
    /// </summary>
    /// <param name="message"></param>
    protected override void ReceiveMessage(ExoforceResponse message)
    {
        Debug.Log(message.success + ": Received " + message.message);
        if (message.success)
        {
            CageInterface.cageIsConnected = isInit;
            var newIcon = "";
            if (isInit)
            {
                newIcon = "CageGreen";
                CageInterface.sentInitRequest = false;
            }
            else
            {
                newIcon = "CageYellow";
            }
            // turn the icon to the corresponding icon
            Widget cageWidget = Manager.Instance.FindWidgetWithID(60);
            cageWidget.GetContext().currentIcon = newIcon;
            // TODO: next line should be commented in to show if the cage is connected, but this isn't working as Unity
            // is not updating the icon.
            //cageWidget.ProcessRosMessage(cageWidget.GetContext());
        }
        else
        {
            if (isInit)
            {
                CageInterface.sentInitRequest = false;
            }
            Debug.LogWarning("Request to Cage unssuccessfull: " + message.message);
        }
    }
}
#endif
