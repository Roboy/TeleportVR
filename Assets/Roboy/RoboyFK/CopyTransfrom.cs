using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransfrom : MonoBehaviour
{

    public Transform controller;

    public bool position = false;
    public bool rotation = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
        if (controller != null && position)
        {
            transform.position = controller.position;
        }
        if (controller != null && rotation)
        {
            transform.rotation = controller.rotation;
        }

    }
}
