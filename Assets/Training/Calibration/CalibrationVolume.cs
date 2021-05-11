using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Training.Calibration
{
    public class CalibrationVolume : MonoBehaviour
    {
        [Tooltip("Tag the colliding object neets to have, in order to trigger calibration")]
        public string requiredTag;
        [Tooltip("Array of TrainingSteps, for which to trigger the calibration")]
        public TutorialSteps.TrainingStep[] requiredSteps;
        [Tooltip("Calibator the calibration will be triggered for")]
        public HandCalibrator calibrator;
        [Tooltip("Renderer of the current object")]
        public MeshRenderer renderer;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // only render the volume, if we're in the right calibration step(s)
            renderer.enabled = requiredSteps.Contains(TutorialSteps.Instance.currentStep);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!requiredSteps.Contains(TutorialSteps.Instance.currentStep) || !other.CompareTag(requiredTag))
            {
                return;
            }
            calibrator.StartCalibration();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!requiredSteps.Contains(TutorialSteps.Instance.currentStep) || !other.CompareTag(requiredTag))
            {
                return;
            }
            calibrator.PauseCalibration();
        }
    }

}
