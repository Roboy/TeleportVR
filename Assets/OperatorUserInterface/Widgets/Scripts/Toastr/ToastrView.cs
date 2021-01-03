using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Widgets.ToastrWidget;

namespace Widgets
{
    public class ToastrView : View
    {
        public readonly int OFFSET = 110;
        public Queue<Toastr> toastrQueue;

        public GameObject toastrDesignPrefab;

        /// <summary>
        /// Init toastr queue in awake to avoid null pointer from Update().
        /// </summary>
        private void Awake()
        {
            toastrQueue = new Queue<Toastr>();
        }

        /// <summary>
        /// Initialize toastr view.
        /// </summary>
        /// <param name="widget"></param>
        public override void Init(Widget widget)
        {
            SetChildWidget(widget.childWidget);

            toastrDesignPrefab = widget.viewDesignPrefab;

            foreach (ToastrTemplate toastrTemplate in ((ToastrWidget)widget).toastrActiveQueue.ToArray())
            {
                CreateNewToastr(toastrTemplate);
            }

            Init(widget.relativeChildPosition, widget.GetContext().unfoldChildDwellTimer, widget.GetContext().onActivate, widget.GetContext().xPositionOffset, widget.GetContext().yPositionOffset, widget.GetContext().scale);
        }

        /// <summary>
        /// Create new Toastr from template and enqueue.
        /// </summary>
        /// <param name="toastrToInstantiate"></param>
        public void CreateNewToastr(ToastrTemplate toastrToInstantiate)
        {
            GameObject toastrGameObject = Instantiate(toastrDesignPrefab);
            toastrGameObject.transform.SetParent(transform, false);
            toastrGameObject.transform.localPosition -= toastrQueue.Count * OFFSET * Vector3.up;

            toastrGameObject.name = toastrToInstantiate.toastrMessage;

            Toastr newToastr = toastrGameObject.GetComponent<Toastr>();
            newToastr.Init(toastrToInstantiate.toastrMessage, toastrToInstantiate.toastrColor, toastrToInstantiate.toastrFontSize);

            toastrQueue.Enqueue(newToastr);
        }

        /// <summary>
        /// Slerp all toastr upwards with delay according to their position.
        /// </summary>
        public void MoveToastrsUp()
        {
            float offsetTime = 0;

            foreach (Toastr toastr in toastrQueue)
            {
                offsetTime += 0.1f;
                toastr.SlerpUp(OFFSET, offsetTime);
            }
        }

        /// <summary>
        /// Destroy top toastr and move all other toastr up.
        /// </summary>
        public void DestroyTopToastr()
        {
            Destroy(toastrQueue.Dequeue().gameObject);
            MoveToastrsUp();
        }

        /// <summary>
        /// Empty. Toastr view is never hidden
        /// </summary>
        /// <param name="relativeChildPosition"></param>
        public override void ShowView(RelativeChildPosition relativeChildPosition)
        {

        }

        /// <summary>
        /// Empty. Toastr view is never hidden
        /// </summary>
        public override void HideView()
        {
            Debug.LogWarning("Toastr widget can not be hidden.");
        }

    }
}