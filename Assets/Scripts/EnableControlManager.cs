using BioIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableControlManager : MonoBehaviour
{
    public BioSegment left_hand;
    public BioSegment right_hand;

    bool right_hand_enabled = false;
    bool left_hand_enabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.GetLeftControllerAvailable()) 
            InputManager.Instance.controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out left_hand_enabled);
        else 
            left_hand_enabled = false;

        if (InputManager.Instance.GetRightControllerAvailable())
            InputManager.Instance.controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out right_hand_enabled);
        else
            right_hand_enabled = false;

        foreach (var objective in left_hand.Objectives)
        {
            objective.enabled = left_hand_enabled;
        }
        foreach (var objective in right_hand.Objectives)
        {
            objective.enabled = right_hand_enabled;
        }

        //    left_hand.EnableControl(left_hand_enabled);
        //right_hand.EnableControl(right_hand_enabled);
    }
}
