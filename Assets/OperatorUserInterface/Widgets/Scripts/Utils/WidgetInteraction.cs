using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animus.ClientSDK;
using Animus.Common;
using AnimusManager;
using UnityEngine;

/// <summary>
/// This script allows to define methods that get called when a widget gets activated.
/// </summary>
namespace Widgets
{
    public class WidgetInteraction : Singleton<WidgetInteraction>
    { 
        [SerializeField] private AnimusClientManager animusManager;

        [SerializeField] private UnityAnimusClient client;

        public bool allowDwellTime;
        
        /// <summary>
        /// Call this function to execute the function with the name given in the argument.
        /// </summary>
        /// <param name="function">The function name.</param>
        public void OnActivate(string function)
        {
            Invoke(function, 0);
        }

        public void ToggleMicro()
        {
            Widget widget = Manager.Instance.FindWidgetWithID(25);
            if (widget.GetContext().currentIcon == "MicroDisabled")
            {
                widget.GetContext().currentIcon = "Micro";
            }
            else
            {
                widget.GetContext().currentIcon = "MicroDisabled";
            }

            widget.ProcessRosMessage(widget.GetContext());
        }

        public void ToggleSpeakers()
        {
            Widget widget = Manager.Instance.FindWidgetWithID(26);
            if (widget.GetContext().currentIcon == "SpeakersOff")
            {
                widget.GetContext().currentIcon = "Speakers";
            }
            else
            {
                widget.GetContext().currentIcon = "SpeakersOff";
            }

            /*string modality = "vision";
            Widget widget = Manager.Instance.FindWidgetWithID(26);
            bool rob_contains_mod = ClientLogic.Instance._chosenRobot.RobotConfig.OutputModalities.Contains(modality);
            bool client_contains_mod = ClientLogic.Instance.requiredModalities.Contains(modality);
            if (rob_contains_mod && client_contains_mod)
            {
                if (widget.GetContext().currentIcon == "SpeakersOff")
                {
                    OpenModalityProto openModality = new OpenModalityProto {ModalityName = modality};
                    Error e = AnimusClient.AnimusClient.OpenModality(ClientLogic.Instance._chosenRobot.RobotId,
                        openModality);
                    print(e.Success);
                    if (!e.Success)
                    {
                        print("Couldn't start " + modality + ": " + e.Description + ", " + e.Code);
                    }

                    widget.GetContext().currentIcon = "Speakers";
                }
                else
                {
                    Error e = AnimusClient.AnimusClient.CloseModality(ClientLogic.Instance._chosenRobot.RobotId,
                        modality);
                    if (!e.Success)
                    {
                        print("Couldn't stop " + modality + ": " + e.Description + ", " + e.Code);
                    }

                    widget.GetContext().currentIcon = "SpeakersOff";
                }
            }
            else
            {
                // TODO: show that modality is not enabled and show on which site it isn't enables (robot or server)
                widget.GetContext().currentIcon = "SpeakersOff";
                if (!rob_contains_mod)
                {
                    print("Robot does not contain modality " + modality);
                }
                else
                {
                    print("Client does not contain modality " + modality);
                }
            }*/

            widget.ProcessRosMessage(widget.GetContext());
        }

        public void ToggleHead()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(41);
            if (latencyTestWidget.GetContext().currentIcon == "HeadGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "HeadYellow";
            }
            else if (latencyTestWidget.GetContext().currentIcon == "HeadYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "HeadRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "HeadGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleRightBody()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(42);
            if (latencyTestWidget.GetContext().currentIcon == "RightBodyGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "RightBodyYellow";
            }
            else if (latencyTestWidget.GetContext().currentIcon == "RightBodyYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "RightBodyRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "RightBodyGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleLeftBody()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(43);
            if (latencyTestWidget.GetContext().currentIcon == "LeftBodyGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftBodyYellow";
            }
            else if (latencyTestWidget.GetContext().currentIcon == "LeftBodyYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftBodyRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "LeftBodyGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleRightHand()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(44);
            if (latencyTestWidget.GetContext().currentIcon == "RightHandGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "RightHandYellow";
            }
            else if (latencyTestWidget.GetContext().currentIcon == "RightHandYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "RightHandRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "RightHandGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleLeftHand()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(45);
            if (latencyTestWidget.GetContext().currentIcon == "LeftHandGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftHandYellow";
            }
            else if (latencyTestWidget.GetContext().currentIcon == "LeftHandYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftHandRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "LeftHandGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleWheelchair()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(46);
            if (latencyTestWidget.GetContext().currentIcon == "WheelchairGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "WheelchairYellow";
            }
            else if (latencyTestWidget.GetContext().currentIcon == "WheelchairYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "WheelchairRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "WheelchairGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
    }
}
