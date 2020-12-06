using UnityEngine;

namespace Widgets
{
    public abstract class Widget : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private WidgetPosition position;

        public GameObject viewDesignPrefab;

        private RosJsonMessage context;
        public int childWidgetId;
        public Widget childWidget;
        public RelativeChildPosition relativeChildPosition;
        
        protected View view;

        /// <summary>
        /// Initialization of widget.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewDesignPrefab"></param>
        public void Init(RosJsonMessage context, GameObject viewDesignPrefab)
        {
            this.context = context;
            id = context.id;
            childWidgetId = context.childWidgetId;
            position = WidgetUtility.StringToWidgetPosition(context.widgetPosition);
            relativeChildPosition = WidgetUtility.StringToRelativeChildPosition(context.relativeChildPosition);

            this.viewDesignPrefab = viewDesignPrefab;
        }

        /// <summary>
        /// Get id.
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return id;
        }

        /// <summary>
        /// Get widget position.
        /// </summary>
        /// <returns></returns>
        public WidgetPosition GetWidgetPosition()
        {
            return position;
        }

        /// <summary>
        /// Get context.
        /// </summary>
        /// <returns></returns>
        public RosJsonMessage GetContext()
        {
            return context;
        }

        /// <summary>
        /// Searches for given child widget by giving the widget manager its id.
        /// </summary>
        private void SearchAndSetChild()
        {
            childWidget = Manager.Instance.FindWidgetWithID(childWidgetId);

            if (childWidget == null)
            {
                Debug.LogWarning("Child widget not found.");
            }
        }

        /// <summary>
        /// Constantly searches for child if it was not found yet. Due to the serial initialization of widgets, the child widget may not be created yet.
        /// Also constantly tries to recreate views, if they were destroyed by transition to construct.
        /// </summary>
        private void Update()
        {
            if (childWidget == null && childWidgetId != 0)
            {
                SearchAndSetChild();
            }
            
            // This is true, everytime the HUD is destroyed
            if (GetView() == null)
            {
                TryToRecreateView();
            }

            UpdateInClass();
        }

        /// <summary>
        /// Pass Update call to inheriting classes for more specific calls.
        /// </summary>
        protected abstract void UpdateInClass();

        /// <summary>
        /// Process incoming ros message
        /// </summary>
        /// <param name="rosMessage"></param>
        public abstract void ProcessRosMessage(RosJsonMessage rosMessage);

        /// <summary>
        /// Create view attatched to hud canvas.
        /// </summary>
        /// <param name="viewParent"></param>
        public void CreateView(GameObject viewParent)
        {
            GameObject viewGameObject = new GameObject(gameObject.name + "View", typeof(RectTransform));
            viewGameObject.transform.SetParent(viewParent.transform, false);
            //view = viewGameObject.AddComponent<ToastrView>();
            view = AddViewComponent(viewGameObject);
            view.Init(this);

            if (position == WidgetPosition.Child)
            {
                view.HideView();
            }
        }

        /// <summary>
        /// Searches for the widget panels by tag. If HUD scene is reactivated, the panels can be found and views can be recreated. This part is a bit hacky and can be improved by using Unity Events.
        /// </summary>
        private void TryToRecreateView()
        {
            GameObject textParent = GameObject.FindGameObjectWithTag("Widgets" + GetWidgetPosition());
            if (textParent != null)
            {
                CreateView(textParent);
            }
        }

        /// <summary>
        /// Get view.
        /// </summary>
        /// <returns></returns>
        public View GetView()
        {
            return view;
        }

        public abstract View AddViewComponent(GameObject viewGameObject);
    }
}
