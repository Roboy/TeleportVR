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
        [Tooltip("Array of TrainingSteps, during which to trigger the calibration")]
        public TutorialSteps.TrainingStep[] requiredTrainingSteps;
        [Tooltip("Array of Steps, during which to trigger the calibration")]
        public HandCalibrator.Step[] requiredCalibrationSteps;
        [Tooltip("Calibator the calibration will be triggered for")]
        public HandCalibrator calibrator;
        [Tooltip("Renderer of the current object")]
        public new MeshRenderer renderer;

        private new bool enabled
        {
            get
            {
                return requiredTrainingSteps.Contains(TutorialSteps.Instance.currentStep)
              && requiredCalibrationSteps.Contains(calibrator.currentStep);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // only render the volume, if we're in the right calibration step(s)
            renderer.enabled = enabled;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!enabled || !other.CompareTag(requiredTag))
            {
                return;
            }
            calibrator.StartCalibration();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!enabled || !other.CompareTag(requiredTag))
            {
                return;
            }
            calibrator.PauseCalibration();
        }
    }

}
