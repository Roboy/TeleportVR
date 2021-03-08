using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public class ToastrWidget : Widget
    {
        // While in construct, incoming toastr are queued and wait to be shown by view in HUD scene.
        public Queue<ToastrTemplate> toastrToInstantiateQueue;

        // These are the toastr that are shown in HUD scene already.
        public Queue<ToastrTemplate> toastrActiveQueue;

        public float duration;
        public Color color;
        public int fontSize;

        private Timer timer;

        /// <summary>
        /// Process incoming new ros message.
        /// </summary>
        /// <param name="rosMessage"></param>
        public override void ProcessRosMessage(RosJsonMessage rosMessage)
        {
            EnqueueNewMessage(rosMessage.toastrMessage, rosMessage.toastrDuration, WidgetUtility.BytesToColor(rosMessage.toastrColor), rosMessage.toastrFontSize);
        }

        /// <summary>
        /// Initialize queues first, to avoid null pointer exception in Update().
        /// </summary>
        private void Awake()
        {
            toastrToInstantiateQueue = new Queue<ToastrTemplate>();
            toastrActiveQueue = new Queue<ToastrTemplate>();
            timer = new Timer();
        }

        /// <summary>
        /// Initializes toastr widget subclass with default values from widget template.
        /// </summary>
        /// <param name="context">Widget context</param>
        /// <param name="viewDesignPrefab">Prefab for toastr views</param>
        public new void Init(RosJsonMessage context, GameObject viewDesignPrefab)
        {
            duration = context.toastrDuration;
            color = WidgetUtility.BytesToColor(context.toastrColor);
            fontSize = context.toastrFontSize;
            
            base.Init(context, viewDesignPrefab);
        }

        /// <summary>
        /// Enqueue new toastr. Use default values, if not specified.
        /// </summary>
        /// <param name="toastrMessage"></param>
        /// <param name="toastrDuration"></param>
        /// <param name="toastrColor"></param>
        /// <param name="toastrFontSize"></param>
        private void EnqueueNewMessage(string toastrMessage, float toastrDuration, Color toastrColor, int toastrFontSize)
        {
            if (toastrMessage.Equals(""))
            {
                return;
            }

            if (toastrColor == null)
            {
                toastrColor = color;
            }

            if (toastrDuration == 0.0f)
            {
                toastrDuration = duration;
            }

            if (toastrFontSize == 0)
            {
                toastrFontSize = fontSize;
            }

            toastrToInstantiateQueue.Enqueue(new ToastrTemplate(toastrMessage, toastrDuration, toastrColor, toastrFontSize));
        }

        /// <summary>
        /// Manage toastr timer.
        /// </summary>
        protected override void UpdateInClass()
        {
            // Check if hud is active to manage timer correctly.
            if (AdditiveSceneManager.CurrentSceneContainsHud())
            {
                if (IsToastrTemplateInQueue() && view != null)
                {
                    ToastrTemplate toastrToInstantiate = toastrToInstantiateQueue.Dequeue();
                    toastrActiveQueue.Enqueue(toastrToInstantiate);
                    ((ToastrView)view).CreateNewToastr(toastrToInstantiate);

                    if (toastrActiveQueue.Count == 1)
                    {
                        timer.SetTimer(toastrActiveQueue.Peek().toastrDuration, DestroyToastr);
                    }
                }

                if (toastrActiveQueue.Count != 0)
                {
                    timer.LetTimePass(Time.deltaTime);
                }
            }
        }

        /// <summary>
        /// If time of top toastr is up, destroy it in queue and in view and set timer of new top toastr. 
        /// </summary>
        private void DestroyToastr()
        {
            ((ToastrView)view).DestroyTopToastr();
            toastrActiveQueue.Dequeue();
            if (toastrActiveQueue.Count != 0)
            {
                timer.SetTimer(toastrActiveQueue.Peek().toastrDuration, DestroyToastr);
            }
        }

        /// <summary>
        /// Checks, if toastr are queued up to be shown.
        /// </summary>
        /// <returns></returns>
        private bool IsToastrTemplateInQueue()
        {
            return (toastrToInstantiateQueue.Count != 0);
        }

        public override View AddViewComponent(GameObject viewGameObject)
        {
            return viewGameObject.AddComponent<ToastrView>();
        }
    
        /// <summary>
        /// Struct to initialize toastr by view.
        /// </summary>
        public class ToastrTemplate
        {
            public string toastrMessage;
            public Color toastrColor;
            public int toastrFontSize;
            public float toastrDuration;

            public ToastrTemplate(string toastrMessage, float toastrDuration, Color toastrColor, int toastrFontSize)
            {
                this.toastrMessage = toastrMessage;
                this.toastrColor = toastrColor;
                this.toastrFontSize = toastrFontSize;
                this.toastrDuration = toastrDuration;
            }
        }
    }
}
