using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    public class IconView : View
    {
        private RawImage image;
        
        /// <summary>
        /// Set current icon.
        /// </summary>
        /// <param name="iconTexture"></param>
        public void SetIcon(Texture2D iconTexture)
        {
            image.texture = iconTexture;
        }

        /// <summary>
        /// Makes view visible. Icon is never child widget until now, so this functionality is still missing here.
        /// </summary>
        /// <param name="relativeChildPosition"></param>
        public override void ShowView(RelativeChildPosition relativeChildPosition)
        {
            image.enabled = true;
        }

        /// <summary>
        /// Hides view.
        /// </summary>
        public override void HideView()
        {
            image.enabled = false;
        }

        /// <summary>
        /// Initializes view.
        /// </summary>
        /// <param name="widget"></param>
        public override void Init(Widget widget)
        {
            GameObject viewDesign = Instantiate(widget.viewDesignPrefab);
            viewDesign.transform.SetParent(this.transform, false);
            image = gameObject.GetComponentInChildren<RawImage>();

            SetChildWidget(widget.childWidget);            
            SetIcon(((IconWidget)widget).currentIcon);

            Init(widget.relativeChildPosition, widget.GetContext().unfoldChildDwellTimer, widget.GetContext().onActivate);
        }
    }
}