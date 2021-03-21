﻿using System.Linq;
using UnityEngine;

namespace Training
{
    public class HandCollectable : MonoBehaviour
    {
        [SerializeField] private string requiredTag;
        [SerializeField] private GameObject objectToRecolor;
        [SerializeField] private int[] requiredSteps;
        [SerializeField] private Color newColor;
        [SerializeField] private Gradient _gradient;
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
            if (requiredSteps.Contains(TutorialSteps.Instance.currentStep) && other.CompareTag(requiredTag))
            {
                collectedSpheres++;
                gameObject.SetActive(false);
                if (collectedSpheres == 2)
                {
                    TutorialSteps.Instance.NextStep();
                }

                /*if (objectToRecolor != null)
                {
                    _renderer.material.color = newColor;
                }*/
            }
        }
    }
}