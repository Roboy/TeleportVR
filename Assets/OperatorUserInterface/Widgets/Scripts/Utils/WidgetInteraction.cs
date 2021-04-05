using AnimusManager;
using UnityEngine;

namespace Widgets
{
    /// <summary>
    /// This script allows to define methods that get called when a widget gets activated or closed.
    /// </summary>
    public class WidgetInteraction : Singleton<WidgetInteraction>
    {
        // Needed if functionality of sound and micro widget gets implemented when animus provides the functionality
        // [SerializeField] private AnimusClientManager animusManager;
        // [SerializeField] private UnityAnimusClient client;

        // determines if interaction with widgets is by clicking or by pointing at them for a certain time
        public bool allowDwellTime;

        // determines if the Displays can be currently moved or not
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
            if (widget.GetContext().currentIcon == "InfoInactive")
            {
                showExplanations = true;
                widget.GetContext().currentIcon = "InfoActive";
            }
            else
            {
                showExplanations = false;
                widget.GetContext().currentIcon = "InfoInactive";
            }

            widget.ProcessRosMessage(widget.GetContext());
        }

        /// <summary>
        /// Allows to toggle if roboy mimics the operators head pose. Currently without functionality, after the head
        /// control has changed. 
        /// </summary>
        public void ToggleHeadControl()
        {
            Widget widget = Manager.Instance.FindWidgetWithID(24);
            if (widget.GetContext().currentIcon == "HeadsetGreen")
            {
                widget.GetContext().currentIcon = "HeadsetYellow";
                // TODO: disable Head segment here
            }
            else
            {
                widget.GetContext().currentIcon = "HeadsetGreen";
                // TODO: enable Head segment here
            }

            widget.ProcessRosMessage(widget.GetContext());
        }

        /// <summary>
        /// Activate or deactivate the micro. Currently without functionality, as animus is not providing it.
        /// </summary>
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

        /// <summary>
        /// Activate or deactivate the speakers. Currently without functionality, as animus is not providing it.
        /// </summary>
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

            // This code block was designed to open or close the audio modality according to the widget state. 
            // As Animus is not yet able to do this, it is commented out and the functionality is not implemented.
            // When animus allows to open and close modalities at runtime, this code might work with a few changes,
            // depending on the animus implementation.
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

        /// <summary>
        /// Set the connection widgets to a specified icon and text.
        /// </summary>
        /// <param name="icon">The wifi icon that should be shown (WifiGreen, WifiRed or WifiYellow)</param>
        /// <param name="message">The message that should be shown when opening the widget.</param>
        public static void SetAnimusStatus(string icon, string message)
        {
            // Set the text
            Widget latencyWidget = Manager.Instance.FindWidgetWithID(33);
            latencyWidget.GetContext().textMessage = message;
            latencyWidget.ProcessRosMessage(latencyWidget.GetContext());

            // Set the icon
            Widget wifiWidget = Manager.Instance.FindWidgetWithID(23);
            wifiWidget.GetContext().currentIcon = icon;
            wifiWidget.ProcessRosMessage(wifiWidget.GetContext());
        }

        /// <summary>
        /// Allow to move the displays.
        /// </summary>
        public void OpenDisplaySettings()
        {
            settingsAreActive = true;
        }

        /// <summary>
        /// Close the mode in which the user can move the Displays.
        /// </summary>
        public void CloseDisplaySettings()
        {
            settingsAreActive = false;
        }

        /// <summary>
        /// Toggle Head icon on activate
        /// </summary>
        public void ToggleHead()
        {
            ChangeIcon(41);
        }

        /// <summary>
        /// Toggle Right Body icon on activate
        /// </summary>
        public void ToggleRightBody()
        {
            ChangeIcon(42);
        }

        /// <summary>
        /// Toggle Left Body icon on activate
        /// </summary>
        public void ToggleLeftBody()
        {
            ChangeIcon(43);
        }

        /// <summary>
        /// Toggle Right Hand icon on activate
        /// </summary>
        public void ToggleRightHand()
        {
            ChangeIcon(44);
        }

        /// <summary>
        /// Toggle Left Hand icon on activate
        /// </summary>
        public void ToggleLeftHand()
        {
            ChangeIcon(45);
        }

        /// <summary>
        /// Toggle Wheelchair icon on activate
        /// </summary>
        public void ToggleWheelchair()
        {
            ChangeIcon(46);
        }
        
        /// <summary>
        /// change the icon on activate. Changes from green to yellow, yellow to red, red to green
        /// </summary>
        /// <param name="id">ID of the icon in the json file</param>
        public static void ChangeIcon(int id)
        {
            Widget widget = Manager.Instance.FindWidgetWithID(id);
            if (widget.GetContext().currentIcon == widget.GetContext().icons[0])
            {
                widget.GetContext().currentIcon = widget.GetContext().icons[1];
            }
            else if (widget.GetContext().currentIcon == widget.GetContext().icons[1])
            {
                widget.GetContext().currentIcon = widget.GetContext().icons[2];
            }
            else
            {
                widget.GetContext().currentIcon = widget.GetContext().icons[0];
            }
            widget.ProcessRosMessage(widget.GetContext());
        }

        public static void SetBodyPartActive(int id, bool active)
        {
            Widget widget = Manager.Instance.FindWidgetWithID(id);
            int iconNr = active ? 1 : 0;
            widget.GetContext().currentIcon = widget.GetContext().icons[iconNr];
            widget.ProcessRosMessage(widget.GetContext());
        }

        /// <summary>
        /// Close the cage, if ROSSHARP is active
        /// </summary>
        private void CloseCage()
        {
#if ROSSHARP
            CageInterface.Instance.CloseCage();
#else
            Debug.LogWarning("Trying to close cage, but RosSharp is not activated.");
#endif
        }
    }
}