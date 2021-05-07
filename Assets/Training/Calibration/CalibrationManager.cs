using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Training.Calibration
{
    public class CalibrationManager : MonoBehaviour
    {
        public HandCalibrator rightCalibrator;
        public HandCalibrator leftCalibrator;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch (TutorialSteps.Instance.currentStep)
            {
                case TutorialSteps.TrainingStep.RIGHT_HAND:
                    rightCalibrator.calibrating = true;
                    if (rightCalibrator.currentStep == HandCalibrator.Step.Done)
                    {
                        TutorialSteps.Instance.NextStep();
                        rightCalibrator.calibrating = false;
                    }
                    break;
                case TutorialSteps.TrainingStep.LEFT_HAND:
                    leftCalibrator.calibrating = true;
                    if (leftCalibrator.currentStep == HandCalibrator.Step.Done)
                    {
                        TutorialSteps.Instance.NextStep();
                        rightCalibrator.calibrating = false;
                    }
                    break;
            }
        }
    }
}
