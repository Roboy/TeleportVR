using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Widgets.TextWidget;

namespace Widgets
{
    public class TextView : View
    {
        private readonly float RELATIVE_OFFSET = 100.0f;

        TextMeshProUGUI textMeshPro;
        RawImage[] images;

        /// <summary>
        /// Hides view by deactivating the TextMeshPro and all background images.
        /// </summary>
        public override void HideView()
        {
            textMeshPro.enabled = false;

            foreach (RawImage image in images)
            {
                image.enabled = false;
            }
        }

        /// <summary>
        /// Show view by activating TextMeshPro, background images and setting the position and parent.
        /// </summary>
        /// <param name="relativeChildPosition"></param>
        public override void ShowView(RelativeChildPosition relativeChildPosition)
        {
            textMeshPro.enabled = true;

            foreach (RawImage image in images)
            {
                image.enabled = true;
            }

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
                    offsetVector = Vector3.left * RELATIVE_OFFSET * 2;
                    break;
                case RelativeChildPosition.Right:
                    offsetVector = Vector3.right * RELATIVE_OFFSET * 2;
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
        /// Change message.
        /// </summary>
        /// <param name="incomingMessageTemplate"></param>
        public void ChangeMessage(TextWidgetTemplate incomingMessageTemplate)
        {
            textMeshPro.text = incomingMessageTemplate.messageToDisplay;
            textMeshPro.color = incomingMessageTemplate.textColor;
            textMeshPro.fontSize = incomingMessageTemplate.textFontSize;
        }

        /// <summary>
        /// Initialization of text view.
        /// </summary>
        /// <param name="widget"></param>
        public override void Init(Widget widget)
        {
            GameObject viewDesign = Instantiate(widget.viewDesignPrefab);
            viewDesign.transform.SetParent(this.transform, false);
            images = gameObject.GetComponentsInChildren<RawImage>();
            textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();

            SetChildWidget(widget.childWidget);
            ChangeMessage(((TextWidget)widget).currentlyDisplayedMessage);

            Init(widget.relativeChildPosition, widget.GetContext().unfoldChildDwellTimer, widget.GetContext().onActivate, widget.GetContext().xPositionOffset, widget.GetContext().yPositionOffset, widget.GetContext().scale);
        }
    }
}