using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationOffset : MonoBehaviour
{

    public Transform controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Derived by experimentation
        Quaternion offset = Quaternion.Euler(-189.118f, -8.403992f, 15.2381f);
        transform.rotation = controller.rotation * offset;
        // Debug.Log(transform.rotation);
    }
}
