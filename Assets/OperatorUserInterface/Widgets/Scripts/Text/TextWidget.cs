using UnityEngine;

namespace Widgets
{
    public class TextWidget : Widget
    {
        public Color color;
        public int fontSize;

        public TextWidgetTemplate currentlyDisplayedMessage;
        
        /// <summary>
        /// Process new incoming ros message.
        /// </summary>
        /// <param name="rosMessage"></param>
        public override void ProcessRosMessage(RosJsonMessage rosMessage)
        {
            TextWidgetTemplate incomingMessageTemplate = new TextWidgetTemplate(rosMessage.textMessage, WidgetUtility.BytesToColor(rosMessage.textColor), rosMessage.textFontSize);
            changeDisplayedMessage(incomingMessageTemplate);
        }

        /// <summary>
        /// Initializes text widget subclass with default values from widget template.
        /// </summary>
        /// <param name="context">Widget context</param>
        /// <param name="viewDesignPrefab">Prefab for icon views</param>
        public new void Init(RosJsonMessage context, GameObject viewDesignPrefab)
        {            
            color = WidgetUtility.BytesToColor(context.textColor);
            fontSize = context.textFontSize;

            TextWidgetTemplate incomingMessageTemplate = new TextWidgetTemplate(context.textMessage, WidgetUtility.BytesToColor(context.textColor), context.textFontSize);
            currentlyDisplayedMessage = incomingMessageTemplate;

            base.Init(context, viewDesignPrefab);
        }

        /// <summary>
        /// Change message in view according to template. Use default values if not specified in template.
        /// </summary>
        /// <param name="incomingMessageTemplate"></param>
        private void changeDisplayedMessage(TextWidgetTemplate incomingMessageTemplate)
        {
            if (incomingMessageTemplate.messageToDisplay.Equals(""))
            {
                return;
            }

            if (incomingMessageTemplate.textColor == null)
            {
                incomingMessageTemplate.textColor = color;
            }
            
            if (incomingMessageTemplate.textFontSize == 0)
            {
                incomingMessageTemplate.textFontSize = fontSize;
            }

            currentlyDisplayedMessage = incomingMessageTemplate;

            ((TextView)view).ChangeMessage(incomingMessageTemplate);
        }

        /// <summary>
        /// Text widgets don't need subclass update calls.
        /// </summary>
        protected override void UpdateInClass()
        {

        }

        /// <summary>
        /// Struct for a new message with its text color and font size, to be passed to view.
        /// </summary>
        public struct TextWidgetTemplate
        {
            public string messageToDisplay;
            public Color textColor;
            public int textFontSize;

            public TextWidgetTemplate(string messageToDisplay, Color textColor, int textFontSize)
            {
                this.messageToDisplay = messageToDisplay;
                this.textColor = textColor;
                this.textFontSize = textFontSize;
            }
        }

        public override View AddViewComponent(GameObject viewGameObject)
        {
            return viewGameObject.AddComponent<TextView>();
        }
    }
}