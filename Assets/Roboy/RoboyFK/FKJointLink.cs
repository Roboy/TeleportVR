using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKJointLink : MonoBehaviour
{

    public Transform controller;
    public BioIK.BioJoint joint;
    public bool approximative = false;

    private Quaternion initialRotation;
    private float initialTarget;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = controller.localRotation;
        initialTarget = (float)joint.X.GetTargetValue();
    }

    // Update is called once per frame
    void Update()
    {
        BioIK.BioJoint.Motion motion = joint.X;
        float angle = 0;
        if (!approximative)
        {
            // Computes the local x-axis amount of controller.localRotation.
            // Based on: https://stackoverflow.com/questions/43606135/split-quaternion-into-axis-rotations
            Quaternion q = controller.localRotation;
            float theta = Mathf.Atan2(q.z, q.w);
            Quaternion zRot = new Quaternion(0, 0, Mathf.Sin(theta), Mathf.Cos(theta));
            angle = Quaternion.Angle(zRot, initialRotation);
        }
        else
        {
            angle = Quaternion.Angle(controller.localRotation, initialRotation);
        }

        angle = controller.localRotation.eulerAngles.z < 180 ? -angle : angle;
        angle = Mathf.Clamp(initialTarget + angle, (float)motion.LowerLimit, (float)motion.UpperLimit);
        joint.X.SetTargetValue(angle);
    }

}
