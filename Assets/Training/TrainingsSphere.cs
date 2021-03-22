using System;
using UnityEngine;

namespace Training
{
    public class TrainingsSphere : MonoBehaviour
    {
        [SerializeField] private string requiredTag;
        [SerializeField] private GameObject objectToRecolor;
        [SerializeField] private int requiredStep;
        [SerializeField] private Color newColor;
        [SerializeField] private Gradient _gradient;
        private MeshRenderer _renderer;

        private float timer;
        [SerializeField] private float dwellTime;

        private void Start()
        {
            _renderer = objectToRecolor.GetComponent<MeshRenderer>();
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (TutorialSteps.Instance.currentStep == requiredStep && other.CompareTag(requiredTag))
            {
                TutorialSteps.Instance.NextStep();

                if (objectToRecolor != null)
                {
                    _renderer.material.color = newColor;
                }
            }
        }*/

        public void WhileLookedAt(TrainingsRaycaster raycaster)
        {
            timer += Time.deltaTime;
            if (timer > dwellTime)
            {
                TutorialSteps.Instance.NextStep();
                raycaster.enabled = false;
            }
            else
            {
                _renderer.material.color = _gradient.Evaluate(timer / dwellTime);
            }
        }
    }
}
