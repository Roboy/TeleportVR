using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationOffset : MonoBehaviour
{

    public bool trackOrientation = true;
    // Controller GameObject to take orientation from
    public Transform controller;
    // Objective Script controlling the hand orienation
    public BioIK.Orientation orientationObjective;

    public bool senseGlove = false;

    // Derived by placing an empty GameObject at the writs position and
    // having the IK goal following it's rotation. 
    // By then applying rotations manually to this game object to visually align Roboy's
    // hand with the Oculus controllers we derived the following constants
    private Quaternion offset;

    // Start is called before the first frame update
    void Start()
    {
        Quaternion senseGloveOffset= Quaternion.Euler(-200f, 0f, 180);
        Quaternion questOffset = Quaternion.Euler(-189.118f, -8.403992f, 15.2381f);
        offset = senseGlove ? senseGloveOffset : questOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackOrientation)
        {
            orientationObjective.Weight = 1;
            transform.rotation = controller.rotation * offset;
        }
        else
        {
            orientationObjective.Weight = 0;
        }
    }
}
