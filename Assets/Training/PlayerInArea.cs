using UnityEngine;

namespace Training
{
    public class PlayerInArea : MonoBehaviour
    {
        [SerializeField] private TutorialSteps.TrainingStep requiredStep;
        [SerializeField] private string requiredTag;
        [SerializeField] private GameObject objectToDisable;

        /// <summary>
        /// Continues to the next step if the player enters this area
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (TutorialSteps.Instance.currentStep == requiredStep && other.CompareTag(requiredTag))
            {
                Debug.Log(requiredTag + " " + other.tag);
                TutorialSteps.Instance.NextStep();
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                }
            }
        }
    }
}
