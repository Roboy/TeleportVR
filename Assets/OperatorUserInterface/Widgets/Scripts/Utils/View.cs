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
        public void Init(RelativeChildPosition relativeChildPosition, float dwellTimerDuration, string onActivate)
        {
            SetRelativeChildPosition(relativeChildPosition);
            this.dwellTimerDuration = dwellTimerDuration;

            if (dwellTimerDuration > 0)
            {
                useDwellTimer = true;
            }

            dwellTimerImage = gameObject.GetComponentInChildren<Image>();
            
            keepChildUnfoldedTimer = new Timer();
            dwellTimer = new Timer();

            this.onActivate = onActivate;
        }

        /// <summary>
        /// Sets child widget position according to parents values and visibility active
        /// </summary>
        public void UnfoldChild()
        {
            if (!childIsActive)
            {
                Invoke(onActivate, 0);
            }
            
            childIsActive = true;
            
            if (childWidget != null)
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

            keepChildUnfoldedTimer.SetTimer(keepOpenDuration, FoldChildIn);
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
            else
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

            keepChildUnfoldedTimer.ResetTimer();
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
            if (keepChildUnfolded)
            {
                keepChildUnfoldedTimer.LetTimePass(Time.deltaTime);
            }

            // Fold child out dwell timer
            if (isLookedAt && useDwellTimer)
            {
                dwellTimer.LetTimePass(Time.deltaTime);

                dwellTimerImage.fillAmount = dwellTimer.GetFraction();
            }
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

        public void ToggleHead()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(54);
            if (latencyTestWidget.GetContext().currentIcon == "HeadGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "HeadYellow";
            }
            else if(latencyTestWidget.GetContext().currentIcon == "HeadYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "HeadRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "HeadGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleWheelchair()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(55);
            if (latencyTestWidget.GetContext().currentIcon == "WheelchairGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "WheelchairYellow";
            }
            else if(latencyTestWidget.GetContext().currentIcon == "WheelchairYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "WheelchairRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "WheelchairGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleLeftHand()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(50);
            if (latencyTestWidget.GetContext().currentIcon == "LeftHandGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftHandYellow";
            }
            else if(latencyTestWidget.GetContext().currentIcon == "LeftHandYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftHandRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "LeftHandGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleRightHand()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(51);
            if (latencyTestWidget.GetContext().currentIcon == "RightHandGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "RightHandYellow";
            }
            else if(latencyTestWidget.GetContext().currentIcon == "RightHandYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "RightHandRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "RightHandGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleRightBody()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(52);
            if (latencyTestWidget.GetContext().currentIcon == "RightBodyGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "RightBodyYellow";
            }
            else if(latencyTestWidget.GetContext().currentIcon == "RightBodyYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "RightBodyRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "RightBodyGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
        
        public void ToggleLeftBody()
        {
            Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(53);
            if (latencyTestWidget.GetContext().currentIcon == "LeftBodyGreen")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftBodyYellow";
            }
            else if(latencyTestWidget.GetContext().currentIcon == "LeftBodyYellow")
            {
                latencyTestWidget.GetContext().currentIcon = "LeftBodyRed";
            }
            else
            {
                latencyTestWidget.GetContext().currentIcon = "LeftBodyGreen";
            }

            latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
        }
    }
}
