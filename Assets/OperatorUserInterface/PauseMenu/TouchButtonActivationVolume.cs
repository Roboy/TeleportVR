using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButtonActivationVolume : MonoBehaviour
{
    public List<System.Action> enterCallbacks = new List<System.Action>();
    public List<System.Action> exitCallbacks = new List<System.Action>();

    //private BoxCollider boxCollider;
    //private Vector3 pos, scale;

    //private void Start()
    //{
    //    boxCollider = gameObject.GetComponent<BoxCollider>();
    //}

    //void Update()
    //{
    //    if (RudderPedals.PresenceDetector.Instance.isPaused)
    //    {
    //        pos = transform.position;
    //        scale = Vector3.Scale(transform.lossyScale, boxCollider.size) / 2;
    //        Collider[] colliders = Physics.OverlapBox(pos, scale, Quaternion.identity, mask);
    //        if (colliders.Length > 0)
    //        {
    //            Debug.Log("Trigger enter");
    //            foreach (var collider in colliders)
    //            {
    //                Debug.Log(collider);
    //            }
    //            foreach (var callback in enterCallbacks)
    //            {
    //                callback();
    //            }
    //        }
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        foreach(var callback in enterCallbacks)
        {
            callback();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit");
        foreach(var callback in exitCallbacks)
        {
            callback();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(pos, scale * 2);
    //}
}
