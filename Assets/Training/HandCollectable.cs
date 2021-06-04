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

        /// <summary>
        /// Collect the sphere if the correct tag/part of the user collides with it
        /// </summary>
        /// <param name="other">The other colliding collider</param>
        private void OnTriggerEnter(Collider other)
        {
            if (requiredSteps.Contains(TutorialSteps.Instance.currentStep) && other.CompareTag(requiredTag))
            {
                collectedSpheres++;
                if (sound != null)
                    TutorialSteps.Instance.ScheduleAudioClip(sound);
                gameObject.SetActive(false);
                TutorialSteps.Instance.NextStep(praise: true);
                Debug.Log("Object collected. Moving on.");
            }
        }
    }
}
