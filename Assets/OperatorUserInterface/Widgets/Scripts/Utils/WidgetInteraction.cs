using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script allows to define methods that get called when a widget gets activated.
/// </summary>
namespace Widgets
{
    public class WidgetInteraction : Singleton<WidgetInteraction>
    {
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
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(25);
            if (latencyTestWidget.GetContext().currentIcon == "MicroDisabled")
            {
                latencyTestWidget.GetContext().currentIcon = "Micro";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "MicroDisabled";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }

        public void ToggleSpeakers()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(26);
            if (latencyTestWidget.GetContext().currentIcon == "SpeakersOff")
            {
                latencyTestWidget.GetContext().currentIcon = "Speakers";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "SpeakersOff";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
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
