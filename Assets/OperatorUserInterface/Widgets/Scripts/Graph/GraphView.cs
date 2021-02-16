using ChartAndGraph;
using TMPro;
using UnityEngine;

namespace Widgets
{
    /// <summary>
    /// Displays and controls how a graph looks like
    /// </summary>
    public class GraphView : View
    {
        private readonly float RELATIVE_OFFSET = 100.0f;
        
        private Material lineMaterial;

        private GraphChart graph;
        private VerticalAxis verticalAxis;
        private HorizontalAxis horizontalAxis;
        private TextMeshProUGUI titleText;

        /// <summary>
        /// Creates a new graph object and initialises it with the properties given in its json file
        /// </summary>
        /// <param name="widget">The corresponding GraphWidget which holds the data</param>
        public override void Init(Widget widget)
        { 
            gameObject.AddComponent<CurvedUI.CurvedUIVertexEffect>();
            GraphWidget graphWidget = (GraphWidget)widget;
            SetChildWidget(graphWidget.childWidget);

            GameObject graphObject = Instantiate(Factory.Instance.graphDesignPrefab);
            graphObject.transform.SetParent(transform);
            graphObject.transform.localScale = Vector3.one * 0.7f;
            graphObject.transform.localPosition = Vector3.down * 45;

            InitGraphObject(graphWidget);
            SetGraphProperties(graphWidget);
            foreach (GraphWidget.Datapoint data in graphWidget.datapoints)
            {
                graph.DataSource.AddPointToCategoryRealtime(graphWidget.name, data.time,
                    data.data);
            }

            Init(widget.relativeChildPosition, widget.GetContext().unfoldChildDwellTimer, widget.GetContext().onActivate, widget.GetContext().onClose, widget.GetContext().xPositionOffset, widget.GetContext().yPositionOffset, widget.GetContext().scale);
        }

        /// <summary>
        /// Sets properties of the newly created graph object and
        /// gets references to important scripts of the new object
        /// </summary>
        /// <param name="graphWidget">The corresponding GraphWidget which holds the data</param>
        private void InitGraphObject(GraphWidget graphWidget)
        {
            graph = GetComponentInChildren<GraphChart>();
            if (graph == null)
            {
                // the ChartGraph info is obtained via the inspector
                Debug.LogWarning("No GraphChart found! Place this script into a graph chart!");
                return;
            }

            lineMaterial = new Material(Shader.Find("Chart/Canvas/Solid"));
            graph.DataSource.AddCategory(graphWidget.name, lineMaterial, 20, new MaterialTiling(false, 20), null, true, null, 20);

            graph.AutoScrollHorizontally = true;
            graph.DataSource.AutomaticHorizontalView = graphWidget.showCompleteHistory;
            if (verticalAxis == null)
            {
                verticalAxis = graph.GetComponent<VerticalAxis>();
                horizontalAxis = graph.GetComponent<HorizontalAxis>();
                horizontalAxis.Format = AxisFormat.Time;
            }
            // has to be changed if the graph gets other TextMeshProUGUI scripts
            titleText = GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Inserts a new datapoint and sets the properties of the graph
        /// </summary>
        /// <param name="widget">The corresponding GraphWidget which holds the data</param>
        public void UpdateView(Widget widget)
        {
            GraphWidget graphWidget = (GraphWidget)widget;
            SetGraphProperties(graphWidget);
            if (graphWidget.datapoints.Count > 0)
            {
                GraphWidget.Datapoint dp = graphWidget.datapoints[graphWidget.datapoints.Count - 1];
                graph.DataSource.AddPointToCategoryRealtime(graphWidget.name, dp.time, dp.data);
            }
        }

        /// <summary>
        /// Sets the properties of the graph such as how many axis labels
        /// get shown and which color the line has
        /// </summary>
        /// <param name="graphWidget">The corresponding GraphWidget which holds the data such as the color</param>
        private void SetGraphProperties(GraphWidget graphWidget)
        {
            SetColor(graphWidget.name, graphWidget.color);
            SetNumLabelsShownX(graphWidget.numXLabels);
            SetNumLabelsShownY(graphWidget.numYLabels);
            titleText.text = graphWidget.name;
            
        }

        /// <summary>
        /// Activates the gameObject of the view, therefore showing it.
        /// If the widget is a child of another view, it sets its parent to the other view and
        /// moves itself next to the parent widget
        /// </summary>
        /// <param name="relativeChildPosition">The direction in which the widget should be, relative to its parent</param>
        public override void ShowView(RelativeChildPosition relativeChildPosition)
        {
            gameObject.SetActive(true);

            Vector3 offsetVector = Vector3.zero;

            switch (relativeChildPosition)
            {
                case RelativeChildPosition.Bottom:
                    offsetVector = Vector3.down * RELATIVE_OFFSET;
                    break;
                case RelativeChildPosition.Top:
                    offsetVector = Vector3.up * RELATIVE_OFFSET;
                    break;
                case RelativeChildPosition.Left:
                    offsetVector = Vector3.left * RELATIVE_OFFSET;
                    break;
                case RelativeChildPosition.Right:
                    offsetVector = Vector3.right * RELATIVE_OFFSET;
                    break;
                case RelativeChildPosition.FixedCenter:
                    transform.SetParent(GameObject.FindGameObjectWithTag("WidgetsCenter").transform, false);
                    return;
                case RelativeChildPosition.Incorrect:
                    return;
            }

            gameObject.AddComponent<CurvedUI.CurvedUIVertexEffect>();

            transform.SetParent(parentView.transform, false);

            transform.localPosition = offsetVector;
        }
        
        /// <summary>
        /// Sets the color of the line of the graph for the given topic
        /// (graphs can just show one line yet, therefore the topic is always the widget name)
        /// </summary>
        /// <param name="topic">the widget name</param>
        /// <param name="c">the new color</param>
        public void SetColor(string topic, Color c)
        {
            Material fill = new Material(lineMaterial);
            fill.color = c;
            graph.DataSource.SetCategoryLine(topic, fill, 5, new MaterialTiling(false, 0));
        }

        /// <summary>
        /// Sets the number of labels shown on the X Axis to num
        /// </summary>
        /// <param name="num">the number of labels that should be shown</param>
        public void SetNumLabelsShownX(int num)
        {
            if (num < 0 || num > 10)
            {
                Debug.LogWarning("Invalid Amount of Labels on X Axis");
            }
            horizontalAxis.MainDivisions.Total = num;
            horizontalAxis.SubDivisions.Total = 1;
        }
        
        /// <summary>
        /// Sets the number of labels shown on the Y Axis to num
        /// </summary>
        /// <param name="num">the number of labels that should be shown</param>
        public void SetNumLabelsShownY(int num)
        {
            if (num <= 0 || num >= 10)
            {
                Debug.LogWarning("Invalid Amount of Labels on Y Axis");
            }
            verticalAxis.MainDivisions.Total = num;
            verticalAxis.SubDivisions.Total = 1;
        }

        /// <summary>
        /// Deactivates the view by deactivating its gameObject
        /// </summary>
        public override void HideView()
        {
            gameObject.SetActive(false);
        }
    }
}
