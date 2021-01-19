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
    }
}
