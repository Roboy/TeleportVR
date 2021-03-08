using System;
using UnityEngine;

namespace Training
{
    public class DetectPlayerCollision : MonoBehaviour
    {
        [SerializeField] private string requiredTag;
        [SerializeField] private GameObject objectToDisable;
        [SerializeField] private int requiredStep;

        private void OnTriggerEnter(Collider other)
        {
            if (TutorialSteps.Instance.currentStep == requiredStep && other.CompareTag(requiredTag))
            {
                TutorialSteps.Instance.NextStep();
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                }
            }
        }
    }
}
