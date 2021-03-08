using System;
using UnityEngine;

namespace Training
{
    public class DetectPlayerCollision : MonoBehaviour
    {
        [SerializeField] private string requiredTag;
        [SerializeField] private GameObject objectToDisable;
        
        
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
            print(gameObject.name + " and " + other.name);
            if (other.CompareTag(requiredTag))
            {
                TutorialSteps.Instance.NextStep();
                objectToDisable.SetActive(false);
            }
        }
    }
}
