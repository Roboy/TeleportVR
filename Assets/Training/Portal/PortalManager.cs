using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [Tooltip("The renderer that displays Roboys view on the portal")]
    [SerializeField] private Renderer backgroundRenderer;

    private void OnTriggerEnter(Collider other)
    {
        // Check if Roboy is entering the Portal
        if (other.CompareTag("RoboyCollider"))
        {
            // Go to the HUD scene
            StateManager.Instance.GoToState(StateManager.States.HUD);
        }
    }

    private void Update()
    {
        // Set the Portal background to the texture streamed to the left eye
        backgroundRenderer.material.mainTexture = UnityAnimusClient.Instance.GetVisionTextures()[0];
    }
}
