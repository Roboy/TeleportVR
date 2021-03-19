using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoboyCollider"))
        {
            StateManager.Instance.GoToState(StateManager.States.HUD);
        }
    }
}
