using UnityEngine;

namespace Training
{
    public class TrainingsRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;

        /// <summary>
        /// Checks if the User is looking at the trainings sphere.
        /// </summary>
        void Update()
        {
            if (TutorialSteps.Instance != null)
            {
                RaycastHit hit;
                if (TutorialSteps.Instance.currentStep == 0 &&
                    Physics.Raycast(transform.position, -1 * transform.up, out hit, 50, layerMask))
                {
                    hit.collider.GetComponent<TrainingsSphere>().WhileLookedAt(this);
                }
            }
        }
    }
}
