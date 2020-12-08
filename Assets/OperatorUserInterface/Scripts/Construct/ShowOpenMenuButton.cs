using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOpenMenuButton : MonoBehaviour
{
    bool menuOpen;
    //this object's local coordinate system is used as reference
    public Transform CompareObject;

    /// <summary>
    /// Set reference to instance.
    /// </summary>
    void Start()
    {
        if (CompareObject == null)
        {
            CompareObject = GameObject.FindGameObjectWithTag("CameraOrigin").transform;
        }
        menuOpen = false;
    }

    /// <summary>
    /// Check if this object (OpenMenuButton) is pointing up (using the CompareObject's local coordinate system)
    /// </summary>
    void FixedUpdate()
    {
        if (!menuOpen)
        {
            if(Vector3.Dot(this.transform.forward, CompareObject.up) < 0)
            {
                for(int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(true);
                }
            } else
            {
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
