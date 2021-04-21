using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKHingeJoint : MonoBehaviour
{

    public Transform controller;

    public Vector3 inputAxis;
    public Vector3 outputAxis;
    public bool constrained;
    public float minRotation;
    public float maxRotation;

    private Quaternion init;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


