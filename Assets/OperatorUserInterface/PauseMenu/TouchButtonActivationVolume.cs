using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButtonActivationVolume : MonoBehaviour
{
    public List<System.Action> enterCallbacks = new List<System.Action>();
    public List<System.Action> exitCallbacks = new List<System.Action>();

    private void OnTriggerEnter(Collider other)
    {
        foreach(var callback in enterCallbacks)
        {
            callback();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach(var callback in exitCallbacks)
        {
            callback();
        }
    }
}
