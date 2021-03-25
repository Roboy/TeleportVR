using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Widgets
{
    public abstract class View : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float keepOpenDuration = 0.5f;
        public float dwellTimerDuration;

        public Widget childWidget;
        public View parentView;
        public Image dwellTimerImage;

        private RelativeChildPosition relativeChildPosition;

        private Timer keepChildUnfoldedTimer;
        private Timer dwellTimer;

        public bool keepChildUnfolded = false;
        public bool dwellTimerActive = false;

        public string onActivate;

        public string onClose;

        public abstract void Init(Widget widget);

        #region FLAGS
        public bool isLookedAt = false;
        public bool childIsActive = false;
        public bool useDwellTimer = false;
        #endregion

        /// <summary>
        /// Called at initialization.
        /// </summary>
        /// <param name="relativeChildPosition"></param>
        /// <param name="dwellTimerDuration"></param>
        public void Init(RelativeChildPosition relativeChildPosition, float dwellTimerDuration, string onActivate, string onClose, float xPositionOffset, float yPositionOffset, float scale)
        {
            SetRelativeChildPosition(relativeChildPosition);
            this.dwellTimerDuration = dwellTimerDuration;

            if (dwellTimerDuration > 0 && WidgetInteraction.Instance.allowDwellTime)
            {
                useDwellTimer = true;
            }

            dwellTimerImage = gameObject.GetComponentInChildren<Image>();
            
            if (WidgetInteraction.Instance.allowDwellTime) {
                keepChildUnfoldedTimer = new Timer();
            }
            dwellTimer = new Timer();

            transform.localPosition = new Vector3(xPositionOffset, yPositionOffset, 0);

            if (scale != 0)
            {
                transform.localScale = new Vector3(scale, scale, scale);
            }

            this.onActivate = onActivate;
            this.onClose = onClose;

            Button btn = GetComponentInChildren<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(ToggleChildFold);
            }
        }

        /// <summary>
        /// If the child is folded in, unfold it, else fold it in.
        /// </summary>
        public void ToggleChildFold() {
            if (childIsActive) {
                FoldChildIn();
            }
            else {
                UnfoldChild();
            }
        }

        /// <summary>
        /// Sets child widget position according to parents values and visibility active
        /// </summary>
        public void UnfoldChild()
        {
            if ((!childIsActive || !WidgetInteraction.Instance.allowDwellTime) && onActivate != null)
            {
                WidgetInteraction.Instance.InvokeFunction(onActivate);
            }

            childIsActive = true;
            
            if (childWidget !=null && ( WidgetInteraction.Instance.showExplanations || !childWidget.trainingInfo)) 
	    {
                childWidget.GetView().SetParentView(this);
                childWidget.GetView().ShowView(relativeChildPosition);
            }

            dwellTimerActive = false;
        }

        /// <summary>
        /// Hides child widget.
        /// </summary>
        public void FoldChildIn()
        {
            if ((childIsActive || !WidgetInteraction.Instance.allowDwellTime) && onClose != null) // is allowDwelltime needed?
            {
                WidgetInteraction.Instance.InvokeFunction(onClose);
            }

            childIsActive = false;

            if (parentView != null)
            {
                parentView.OnSelectionExit();
                ResetDwellTimer();
            }

            if (childWidget != null)
            {
                childWidget.GetView().SetParentView(null);
                childWidget.GetView().HideView();
            }

            keepChildUnfolded = false;
        }

        /// <summary>
        /// Resets dwell timer to zero.
        /// </summary>
        public void ResetDwellTimer()
        {
            dwellTimer.ResetTimer();
            dwellTimerActive = false;
        }

        /// <summary>
        /// Called when pointer or gaze enters childs view.
        /// </summary>
        public void OnSelectionChildEnter()
        {
            keepChildUnfolded = false;
        }

        /// <summary>
        /// Called when pointer or gaze exits childs view.
        /// </summary>
        public void OnSelectionChildExit()
        {
            keepChildUnfolded = true;
        }

        /// <summary>
        /// Called when pointer or gaze enters this view.
        /// </summary>
        public void OnSelectionEnter()
        {
            isLookedAt = true;

            if (keepChildUnfoldedTimer != null) {
                keepChildUnfoldedTimer.SetTimer(keepOpenDuration, FoldChildIn);
            }
            keepChildUnfolded = false;

            if (parentView != null)
            {
                parentView.OnSelectionChildEnter();
            }

            if (useDwellTimer)
            {
                dwellTimer.SetTimer(dwellTimerDuration, UnfoldChild);
                dwellTimerActive = true;
            }
            else if (WidgetInteraction.Instance.allowDwellTime)
            {
                UnfoldChild();
            }
        }


        /// <summary>
        /// Called when pointer or gaze exits this view.
        /// </summary>
        public void OnSelectionExit()
        {
            isLookedAt = false;

            if (parentView != null)
            {
                OnSelectionChildExit();
            }

            if (keepChildUnfoldedTimer != null) {
                keepChildUnfoldedTimer.ResetTimer();
            }
            keepChildUnfolded = true;

            if (useDwellTimer)
            {
                dwellTimerActive = false;
                dwellTimerImage.fillAmount = 0.0f;
            }
        }

        /// <summary>
        /// Set relative child position.
        /// </summary>
        /// <param name="relativeChildPosition"></param>
        public void SetRelativeChildPosition(RelativeChildPosition relativeChildPosition)
        {
            this.relativeChildPosition = relativeChildPosition;
        }

        /// <summary>
        /// Set all view components visible.
        /// </summary>
        /// <param name="relativeChildPosition"></param>
        public abstract void ShowView(RelativeChildPosition relativeChildPosition);

        /// <summary>
        /// Sets all view components invisible
        /// </summary>
        public abstract void HideView();

        /// <summary>
        /// Set child widget.
        /// </summary>
        /// <param name="childWidget"></param>
        public void SetChildWidget(Widget childWidget)
        {
            this.childWidget = childWidget;
        }

        /// <summary>
        /// Set parent view.
        /// </summary>
        /// <param name="parentView"></param>
        public void SetParentView(View parentView)
        {
            this.parentView = parentView;
        }

        /// <summary>
        /// Managing timers.
        /// </summary>
        public void Update()
        {
            // Folding child in again timer
            if (keepChildUnfolded && keepChildUnfoldedTimer != null)
            {
                keepChildUnfoldedTimer.LetTimePass(Time.deltaTime);
            }

            // Fold child out dwell timer
            if (isLookedAt && useDwellTimer)
            {
                dwellTimer.LetTimePass(Time.deltaTime);

                dwellTimerImage.fillAmount = dwellTimer.GetFraction();
            }

            
            // Input to activate or deactivate Highlights by Button down
            if (Input.GetKeyDown(KeyCode.C))
            {
                highlight(51);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                dehighlight(51);
            }
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                highlight(52);
            }
            else if (Input.GetKeyUp(KeyCode.V))
            {
                dehighlight(52);
            }
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                highlight(53);
            }
            else if (Input.GetKeyUp(KeyCode.B))
            {
                dehighlight(53);
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                highlight(54);
            }
            else if (Input.GetKeyUp(KeyCode.N))
            {
                dehighlight(54);
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                highlight(55);
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                dehighlight(55);
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                highlight(56);
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                dehighlight(56);
            }

        }
        
        public void highlight(int id)
        {
            Widget widget = Manager.Instance.FindWidgetWithID(id);
            widget.GetContext().currentIcon = widget.GetContext().icons[1];
            widget.ProcessRosMessage(widget.GetContext());
        }

        public void dehighlight(int id)
        {
            Widget widget = Manager.Instance.FindWidgetWithID(id);
            widget.GetContext().currentIcon = widget.GetContext().icons[0];
            widget.ProcessRosMessage(widget.GetContext());
        }

        /// <summary>
        /// If mouse enters view.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSelectionEnter();
        }

        /// <summary>
        /// If mouse exits view.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            OnSelectionExit();
        }
    }
}
