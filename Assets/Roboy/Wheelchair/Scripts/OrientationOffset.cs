using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationOffset : MonoBehaviour
{
    // Controller GameObject to take orientation from
    public Transform controller;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        // Derived by placing an empty GameObject at the writs position and
        // having the IK goal following it's rotation. 
        // By then applying rotations manually to this game object to visually align Roboy's
        // hand with the Oculus controllers we derived the following constants
        const float rx = -189.118f, ry = -8.403992f, rz = 15.2381f;
        Quaternion offset = Quaternion.Euler(rx, ry, rz);
        transform.rotation = controller.rotation * offset;
    }
}
