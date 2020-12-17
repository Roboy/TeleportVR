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
    }
}
