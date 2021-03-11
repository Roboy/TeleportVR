using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelchairStateManager : MonoBehaviour
{
    [SerializeField] private GameObject[] WheelchairModels;
    [SerializeField] private GameObject UpperBody;

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
        UpperBody.SetActive(StateManager.Instance.currentState != StateManager.States.HUD);
    }
}
