using System;
using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public class GraphWidget : Widget
    {
        public readonly int SIZE = 100;

        public Color color = Color.white;

        public int numXLabels = 2;
        public int numYLabels = 3;

        public bool showCompleteHistory;

        public List<Datapoint> datapoints;
        
        /// <summary>
        /// Creates a new list to store datapoints
        /// </summary>
        public void Awake()
        {
            datapoints = new List<Datapoint>();
        }

        /// <summary>
        /// Stores the initial data.
        /// </summary>
        /// <param name="context">the message which contains the initial data</param>
        /// <param name="viewDesignPrefab">the prefab that should be instantiated to show the widget</param>
        public new void Init(RosJsonMessage context, GameObject viewDesignPrefab)
        {
            color = WidgetUtility.BytesToColor(context.graphColor);
            numXLabels = context.xDivisionUnits;
            numYLabels = context.yDivisionUnits;
            showCompleteHistory = context.showCompleteHistory;
            
            base.Init(context, viewDesignPrefab);
        }

        /// <summary>
        /// Gets called when a new RosMessage arrives for the widget. Adds a new datapoint and/or updates
        /// the widget properties
        /// </summary>
        /// <param name="rosMessage">the message which contains the live data point</param>
        public override void ProcessRosMessage(RosJsonMessage rosMessage)
        {
            if (rosMessage.graphColor != null && rosMessage.graphColor.Length == 4)
            {
                color = WidgetUtility.BytesToColor(rosMessage.graphColor);
            }
            DateTime dt = DateTime.Now;
            if (rosMessage.graphTimestamp != 0)
            {
                DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
                dt = epochStart.AddSeconds(rosMessage.graphTimestamp);
            }
            if (rosMessage.graphValue != 0)
            {
                AddDatapoint(new Datapoint(dt, rosMessage.graphValue));
            }

            showCompleteHistory = rosMessage.showCompleteHistory;
        }

        /// <summary>
        /// Stores the new datapoint and updates the view
        /// </summary>
        /// <param name="newDatapoint">the latest datapoint</param>
        public void AddDatapoint(Datapoint newDatapoint)
        {
            if (datapoints.Count == SIZE)
            {
                datapoints.RemoveAt(0);
            }

            datapoints.Add(newDatapoint);

            if (view != null)
            {
                ((GraphView)view).UpdateView(this);
            }
        }

        /// <summary>
        /// Update function that gets called each frame. The graph just changes when a new message comes in,
        /// therefore it is empty
        /// </summary>
        protected override void UpdateInClass()
        {

        }

        /// <summary>
        /// Attaches the GraphView to the corresponding GameObject
        /// </summary>
        /// <param name="viewGameObject">the GameObject the new view should be attached to</param>
        /// <returns>the reference to the created View</returns>
        public override View AddViewComponent(GameObject viewGameObject)
        {
            return viewGameObject.AddComponent<GraphView>();
        }

        /// <summary>
        /// the necessary data needed for a point on the graph
        /// </summary>
        public struct Datapoint
        {
            public DateTime time;
            public float data;

            public Datapoint(DateTime time, float newDatapoint)
            {
                this.time = time;
                this.data = newDatapoint;
            }
        }
    }
}