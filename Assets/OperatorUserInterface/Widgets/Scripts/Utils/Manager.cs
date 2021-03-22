using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public class Manager : Singleton<Manager>
    {
        public List<Widget> widgets;
        //public RosSubscriber rosSubscriber;

        /// <summary>
        /// Create all registered widgets from their template files.
        /// </summary>
        void Start()
        {
            //rosSubscriber = GameObject.FindGameObjectWithTag("RosManager").GetComponent<RosSubscriber>();
            widgets = Factory.Instance.CreateWidgetsAtStartup();
        }

        /// <summary>
        /// Pull new ros messages from subscriber. Also used for easy mocking.
        /// </summary>
        void Update()
        {
            /*if (!rosSubscriber.IsEmpty())
            {
                ForwardMessageToWidget(rosSubscriber.DequeueInterfaceMessage());
            }*/ // uncomment for ros to work

            // Mock Area
            /*if (Input.GetKeyDown(KeyCode.T))
            {
                Widget testToastrWidget = FindWidgetWithID(10);
                testToastrWidget.ProcessRosMessage(testToastrWidget.GetContext());
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Widget testGraphWidget = FindWidgetWithID(1);
                testGraphWidget.GetContext().graphValue = Random.Range(15, 30);
                testGraphWidget.ProcessRosMessage(testGraphWidget.GetContext());
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Widget iconTestWidget = FindWidgetWithID(29);
                iconTestWidget.GetContext().currentIcon = "OrangeJuice";
                iconTestWidget.ProcessRosMessage(iconTestWidget.GetContext());
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                Widget latencyTestWidget = FindWidgetWithID(33);
                latencyTestWidget.GetContext().textMessage = "Latency: 25ms";
                latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
            }
        }*/
        }

        /// <summary>
        /// Forwared a given ros message to the widget with the corresponding id.
        /// </summary>
        /// <param name="rosMessage">Incoming ros message to be forwarded</param>
        private void ForwardMessageToWidget(RosJsonMessage rosMessage)
        {
            if (rosMessage == null)
            {
                Debug.LogWarning("RosMessage was null");
                return;
            }

            Widget widget = FindWidgetWithID(rosMessage.id);

            if (widget == null)
            {
                Debug.LogWarning("Message could not be forwarded.");
                return;
            }

            widget.ProcessRosMessage(rosMessage);
        }

        /// <summary>
        /// Search for a registered widget by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found widget or null</returns>
        public Widget FindWidgetWithID(int id)
        {
            foreach (Widget widget in widgets)
            {
                if (widget.GetID() == id)
                {
                    return widget;
                }
            }

            Debug.LogWarning("No widget with id " + id + " found.");
            return null;
        }
    }
}