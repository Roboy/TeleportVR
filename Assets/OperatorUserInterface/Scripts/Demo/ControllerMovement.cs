using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerMovement : MonoBehaviour {
    // a reference to the action
    /*public SteamVR_Action_Vector2 trackpadMovement;
    public SteamVR_Action_Boolean triggerClick;

    // a reference to the hand
    public SteamVR_Input_Sources handType;

    // Start is called before the first frame update
    void Start() {
        trackpadMovement.AddOnChangeListener(OnTrackpadChange, handType);
        trackpadMovement.AddOnAxisListener(OnAxisChange, handType);
        trackpadMovement.AddOnUpdateListener(OnUpdate, handType);
        
        triggerClick.AddOnStateDownListener(OnTriggerStateDown, handType);
    }

    private void OnTriggerStateDown(SteamVR_Action_Boolean fromaction, SteamVR_Input_Sources fromsource) {
        Debug.Log("Trigger Pressed");
    }

    private void OnUpdate(SteamVR_Action_Vector2 fromaction, SteamVR_Input_Sources fromsource, Vector2 axis, Vector2 delta) {
        Debug.Log("Controller Movement (3). " + axis.ToString() + " | " + delta.ToString());
    }

    private void OnAxisChange(SteamVR_Action_Vector2 fromaction, SteamVR_Input_Sources fromsource, Vector2 axis, Vector2 delta) {
        Debug.Log("Controller Movement (2). " + axis.ToString() + " | " + delta.ToString());
    }

    private void OnTrackpadChange(SteamVR_Action_Vector2 fromaction, SteamVR_Input_Sources fromsource, Vector2 axis, Vector2 delta) {
        Debug.Log("Controller Movement. " + axis.ToString() + " | " + delta.ToString());
    }*/

}