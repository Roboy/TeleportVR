using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButtonActivationVolume : MonoBehaviour
{
    public List<System.Action> enterCallbacks = new List<System.Action>();
    public List<System.Action> exitCallbacks = new List<System.Action>();

    public string[] colliderTags;


    private void OnTriggerEnter(Collider other)
    {

        if (!ColliderHasTags(other))
            return;
        Debug.Log("TouchButtonActivationVolume enter");
        foreach(var callback in enterCallbacks)
        {
            callback();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TouchButtonActivationVolume exit");
        if (!ColliderHasTags(other))
            return;
        foreach(var callback in exitCallbacks)
        {
            callback();
        }
    }

    private bool ColliderHasTags(Collider collider)
    {
        foreach(var tag in colliderTags)
        {
            if (collider.CompareTag(tag))
                return true;
        }
        return false;
    }
}
