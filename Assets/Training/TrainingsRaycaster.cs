using UnityEngine;

namespace Training
{
    public class TrainingsRaycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            if (TutorialSteps.Instance.currentStep == 0 && 
                Physics.Raycast(transform.position, -1 * transform.up, out hit, 50, layerMask))
            {
                hit.collider.GetComponent<TrainingsSphere>().WhileLookedAt(this);
            }
        }
    }
}
