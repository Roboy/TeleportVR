using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairStateManager : Singleton<WheelchairStateManager>
{
    [SerializeField] private GameObject[] WheelchairModels;
    [SerializeField] private GameObject UpperBody, Legs;

    void Update()
    {
        foreach (GameObject WheelchairModel in WheelchairModels)
        {
            if (WheelchairModel != null)
            {
                WheelchairModel.SetActive(StateManager.Instance.currentState != StateManager.States.HUD);
            }
        }
        // if BioIK is needed for real roboy, only the meshes might need to be disabled, but for now just disable it all
        //UpperBody.SetActive(StateManager.Instance.currentState != StateManager.States.HUD);
        foreach (Renderer r in UpperBody.GetComponentsInChildren<Renderer>())
        {
            r.enabled = StateManager.Instance.currentState != StateManager.States.HUD;
            Color c = r.material.color;
            float a = StateManager.Instance.currentState == StateManager.States.HUD ? 0.4f : 1f;
            r.material.color = c;
        }
        //Legs.SetActive(StateManager.Instance.currentState != StateManager.States.HUD);
        foreach (Renderer r in Legs.GetComponentsInChildren<Renderer>())
        {
            r.enabled = StateManager.Instance.currentState != StateManager.States.HUD;
            Color c = r.material.color;
            float a = StateManager.Instance.currentState == StateManager.States.HUD ? 0.4f : 1f;
            r.material.color = c;
        }
    }
}
