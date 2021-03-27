using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private Renderer backgroundRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoboyCollider"))
        {
            StateManager.Instance.GoToState(StateManager.States.HUD);
        }
    }

    private void Update()
    {
        backgroundRenderer.material.mainTexture = UnityAnimusClient.Instance.GetVisionTextures()[0];
    }
}
