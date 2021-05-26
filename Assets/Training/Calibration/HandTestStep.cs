using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG.Calibration;

namespace Training.Calibration
{
    public class HandTestStep : MonoBehaviour
    {
        public HandCalibrator leftCalibrator;
        public HandCalibrator handCalibrator;

        public List<System.Action> callbacks;
        // Start is called before the first frame update
        void Start()
        {
            callbacks = new List<System.Action>();
        }

        // Update is called once per frame
        void Update()
        {
            if (TutorialSteps.Instance.currentStep != TutorialSteps.TrainingStep.HAND_TEST)
            {
                return;
            }

            CalibrationPose a;
            a.CalibrateParameters
            

        }

        public void OnDone(System.Action callback)
        {

        }
    }

}
