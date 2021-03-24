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

        public static bool settingsAreActive;
	public bool showExplanations = false;
        
        /// <summary>
        /// Call this function to execute the function with the name given in the argument.
        /// </summary>
        /// <param name="function">The function name.</param>
        public void InvokeFunction(string function)
        {
            Invoke(function, 0);
        }

        public void ToggleInformation()
        {
            Widget widget = Manager.Instance.FindWidgetWithID(214);
            if (widget.GetContext().currentIcon == "infoInactive")
            {
		showExplanations = true;
                widget.GetContext().currentIcon = "infoActive";
		
		
            }
            else
            {
		showExplanations = false;
                widget.GetContext().currentIcon = "infoInactive";
		
            }

            widget.ProcessRosMessage(widget.GetContext());
        }

        public void ToggleHeadControl()
        {
            Widget widget = Manager.Instance.FindWidgetWithID(24);
            if (widget.GetContext().currentIcon == "HeadsetGreen")
            {
                widget.GetContext().currentIcon = "HeadsetYellow";
		
            }
            else
            {
                widget.GetContext().currentIcon = "HeadsetGreen";
		
            }

            widget.ProcessRosMessage(widget.GetContext());
        }

        public void ToggleMicro()
        {
            Widget widget = Manager.Instance.FindWidgetWithID(25);
            if (widget.GetContext().currentIcon == "MicroDisabled")
            {
                widget.GetContext().currentIcon = "Micro";
                Microphone.Start(null, true, 10, 44100);
            }
            else
            {
                widget.GetContext().currentIcon = "MicroDisabled";
                Microphone.End(null);
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

        public void OpenDisplaySettings() {
            settingsAreActive = true;
            print("Opened Display settings!");
        }

        public void CloseDisplaySettings() {
            settingsAreActive = false;
            print("Closed Display settings!");
        }

        public void ToggleHead()
        {
            Widget headWidget = Manager.Instance.FindWidgetWithID(41);
            if (headWidget.GetContext().currentIcon == "HeadGreen")
            {
                headWidget.GetContext().currentIcon = "HeadYellow";
            }
            else if (headWidget.GetContext().currentIcon == "HeadYellow")
            {
                headWidget.GetContext().currentIcon = "HeadRed";
            }
            else
            {
                headWidget.GetContext().currentIcon = "HeadGreen";
            }

            headWidget.ProcessRosMessage(headWidget.GetContext());
        }
        
        public void ToggleRightBody()
        {
            Widget rightBodyWidget = Manager.Instance.FindWidgetWithID(42);
            if (rightBodyWidget.GetContext().currentIcon == "RightBodyGreen")
            {
                rightBodyWidget.GetContext().currentIcon = "RightBodyYellow";
            }
            else if (rightBodyWidget.GetContext().currentIcon == "RightBodyYellow")
            {
                rightBodyWidget.GetContext().currentIcon = "RightBodyRed";
            }
            else
            {
                rightBodyWidget.GetContext().currentIcon = "RightBodyGreen";
            }

            rightBodyWidget.ProcessRosMessage(rightBodyWidget.GetContext());
        }
        
        public void ToggleLeftBody()
        {
            Widget leftBodyWidget = Manager.Instance.FindWidgetWithID(43);
            if (leftBodyWidget.GetContext().currentIcon == "LeftBodyGreen")
            {
                leftBodyWidget.GetContext().currentIcon = "LeftBodyYellow";
            }
            else if (leftBodyWidget.GetContext().currentIcon == "LeftBodyYellow")
            {
                leftBodyWidget.GetContext().currentIcon = "LeftBodyRed";
            }
            else
            {
                leftBodyWidget.GetContext().currentIcon = "LeftBodyGreen";
            }

            leftBodyWidget.ProcessRosMessage(leftBodyWidget.GetContext());
        }
        
        public void ToggleRightHand()
        {
            Widget rightHandWidget = Manager.Instance.FindWidgetWithID(44);
            if (rightHandWidget.GetContext().currentIcon == "RightHandGreen")
            {
                rightHandWidget.GetContext().currentIcon = "RightHandYellow";
            }
            else if (rightHandWidget.GetContext().currentIcon == "RightHandYellow")
            {
                rightHandWidget.GetContext().currentIcon = "RightHandRed";
            }
            else
            {
                rightHandWidget.GetContext().currentIcon = "RightHandGreen";
            }

            rightHandWidget.ProcessRosMessage(rightHandWidget.GetContext());
        }
        
        public void ToggleLeftHand()
        {
            Widget leftHandWidget = Manager.Instance.FindWidgetWithID(45);
            if (leftHandWidget.GetContext().currentIcon == "LeftHandGreen")
            {
                leftHandWidget.GetContext().currentIcon = "LeftHandYellow";
            }
            else if (leftHandWidget.GetContext().currentIcon == "LeftHandYellow")
            {
                leftHandWidget.GetContext().currentIcon = "LeftHandRed";
            }
            else
            {
                leftHandWidget.GetContext().currentIcon = "LeftHandGreen";
            }

            leftHandWidget.ProcessRosMessage(leftHandWidget.GetContext());
        }
        
        public void ToggleWheelchair()
        {
            Widget wheelchairWidget = Manager.Instance.FindWidgetWithID(46);
            if (wheelchairWidget.GetContext().currentIcon == "WheelchairGreen")
            {
                wheelchairWidget.GetContext().currentIcon = "WheelchairYellow";
            }
            else if (wheelchairWidget.GetContext().currentIcon == "WheelchairYellow")
            {
                wheelchairWidget.GetContext().currentIcon = "WheelchairRed";
            }
            else
            {
                wheelchairWidget.GetContext().currentIcon = "WheelchairGreen";
            }

            wheelchairWidget.ProcessRosMessage(wheelchairWidget.GetContext());
        }

        private void CloseCage()
        {
            CageInterface.Instance.CloseCage();
        }
    }
}
