using System;

namespace Widgets
{
    [Serializable]
    public class RosJsonMessage
    {        
        public int id; 

        #region General        
        public string title;
        public string type;
        public string widgetPosition;
        public string relativeChildPosition;
        public float unfoldChildDwellTimer;
        public int childWidgetId;
        public int timestamp;
        public string onActivate;
        #endregion

        #region Graph
        // Time passed in s since 1/1/1970
        public double graphTimestamp;  // MANDATORY FOR GRAPH
        public float graphValue;    // MANDATORY FOR GRAPH
        public byte[] graphColor;
        public int xDivisionUnits;
        public int yDivisionUnits;
        public bool showCompleteHistory;
        #endregion

        #region Toastr
        public string toastrMessage;    
        public int toastrFontSize;
        public byte[] toastrColor;
        public float toastrDuration;
        #endregion

        #region Text
        public string textMessage;      
        public int textFontSize;
        public byte[] textColor;
        #endregion

        #region Icon
        public string currentIcon;      
        public string[] icons;
        #endregion
        
        /// <summary>
        /// Internal constructor for gaph widget json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="graphDatapoint"></param>
        /// <param name="graphTimestamp"></param>
        /// <param name="graphColor"></param>
        private RosJsonMessage(int id, float graphDatapoint, double graphTimestamp, byte[] graphColor)
        {
            this.id = id;
            this.graphValue = graphDatapoint;
            this.graphColor = graphColor;
            this.graphTimestamp = graphTimestamp;
        }

        /// <summary>
        /// Internal constructor for icon widget json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="graphDatapoint"></param>
        /// <param name="graphTimestamp"></param>
        /// <param name="graphColor"></param>
        private RosJsonMessage(int id, string currentIcon)
        {
            this.id = id;
            this.currentIcon = currentIcon;
        }
        
        /// <summary>
        /// Internal constructor for toastr widget json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="graphDatapoint"></param>
        /// <param name="graphTimestamp"></param>
        /// <param name="graphColor"></param>
        private RosJsonMessage(int id, string toastrMessage, float toastrDuration, byte[] col)
        {
            this.id = id;
            this.toastrMessage = toastrMessage;
            this.toastrDuration = toastrDuration;
            this.toastrColor = col;
        }

        /// <summary>
        /// Internal constructor for registering widget
        /// </summary>
        /// <param name="id"></param>
        /// <param name="graphDatapoint"></param>
        /// <param name="graphTimestamp"></param>
        /// <param name="graphColor"></param>
        public RosJsonMessage(int id, string title, string type, string widgetPosition, string relativeChildPosition, int childWidgetId, string textMessage, int textFontSize, byte[] textColor) : this(id, title)
        {
            this.type = type;
            this.widgetPosition = widgetPosition;
            this.relativeChildPosition = relativeChildPosition;
            this.childWidgetId = childWidgetId;
            this.textMessage = textMessage;
            this.textFontSize = textFontSize;
            this.textColor = textColor;
        }

        /// <summary>
        /// Factory method to create text json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="widgetPosition"></param>
        /// <param name="relativeChildPosition"></param>
        /// <param name="childWidgetId"></param>
        /// <param name="textMessage"></param>
        /// <param name="textFontSize"></param>
        /// <param name="textColor"></param>
        /// <returns></returns>
        public static RosJsonMessage CreateTextMessage(int id, string textMessage, int textFontSize, byte[] textColor)
        {
            return new RosJsonMessage(id, textMessage, textFontSize, textColor);
        }

        /// <summary>
        /// Factory method to create graph json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="graphDatapoint"></param>
        /// <param name="timestamp"></param>
        /// <param name="graphColor"></param>
        /// <returns></returns>
        public static RosJsonMessage CreateGraphMessage(int id, float graphDatapoint, double timestamp, byte[] graphColor)
        {
            return new RosJsonMessage(id, graphDatapoint, timestamp, graphColor);
        }

        /// <summary>
        /// Factory method to create icon json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentIcon"></param>
        /// <returns></returns>
        public static RosJsonMessage CreateIconMessage(int id, string currentIcon)
        {
            return new RosJsonMessage(id, currentIcon);
        }

        /// <summary>
        /// Factory method to create toastr json message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static RosJsonMessage CreateToastrMessage(int id, string text, float duration, byte[] col)
        {
            return new RosJsonMessage(id, text, duration, col);
        }
    }
}