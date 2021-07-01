using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Training.Calibration
{
#if SENSEGLOVE
    public class GrabInteraction : MonoBehaviour
    {

        public SG.SG_Grabable grabable;

        public TutorialSteps.TrainingStep[] requiredSteps;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (requiredSteps.Contains(TutorialSteps.Instance.currentStep) && grabable.IsGrabbed())
            {
                TutorialSteps.Instance.NextStep();
            }
        }
    }
#endif
}
