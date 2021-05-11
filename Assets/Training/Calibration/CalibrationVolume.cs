using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Training
{
    public class CalibrationVolume : MonoBehaviour
    {

        public string requiredTag;
        [SerializeField] private TutorialSteps.TrainingStep[] requiredSteps;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!requiredSteps.Contains(TutorialSteps.Instance.currentStep) || !other.CompareTag(requiredTag))
            {

            }
        }
    }

}
