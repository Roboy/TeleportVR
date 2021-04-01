using System.Linq;
using UnityEngine;

namespace Training
{
    public class HandCollectable : MonoBehaviour
    {
        [SerializeField] private string requiredTag;
        [SerializeField] private GameObject objectToRecolor;
        [SerializeField] private TutorialSteps.TrainingStep[] requiredSteps;
        [SerializeField] private Color newColor;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private AudioClip sound;
        private MeshRenderer _renderer;

        private static int collectedSpheres;

        private float timer;
        [SerializeField] private float dwellTime;

        private void Start()
        {
            _renderer = objectToRecolor.GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag);
            Debug.Log("current step: " + TutorialSteps.Instance.currentStep + " required step: " + requiredSteps[0]);
            Debug.Log("current tag: " + other.tag + " required tag: " + requiredTag);
            if (requiredSteps.Contains(TutorialSteps.Instance.currentStep) && other.CompareTag(requiredTag))
            {

                collectedSpheres++;
                if (sound != null)
                    TutorialSteps.Instance.ScheduleAudioClip(sound);
                gameObject.SetActive(false);
                //if (collectedSpheres == 2)
                //{
                TutorialSteps.Instance.NextStep(praise: true);
                Debug.Log("Object collected. Moving on.");
                //}

                /*if (objectToRecolor != null)
                {
                    _renderer.material.color = newColor;
                }*/
            }
        }
    }
}
