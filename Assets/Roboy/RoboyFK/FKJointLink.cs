using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FKJointLink : MonoBehaviour
{
    public enum RotationAxis
    {
        X, Y, Z
    }

    public Transform controller;
    public BioIK.BioJoint joint;
    public bool approximative = false;
    public RotationAxis axis = RotationAxis.Z;
    public bool invert = false;

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
        float angle;
        if (!approximative)
        {
            Quaternion q = controller.localRotation;
            Quaternion axisRot = ComponentQuaternion(q, axis);
            angle = Quaternion.Angle(axisRot, initialRotation);
        }
        else
        {
            angle = Quaternion.Angle(controller.localRotation, initialRotation);
        }

        angle = controller.localRotation.eulerAngles.z < 180 ? -angle : angle;
        angle = invert ? -angle : angle;
        angle = Mathf.Clamp(initialTarget + angle, (float)motion.LowerLimit, (float)motion.UpperLimit);
        joint.X.SetTargetValue(angle);
    }

// Computes the per-axis contribution of q.
// Based on: https://stackoverflow.com/questions/43606135/split-quaternion-into-axis-rotations
    private static Quaternion ComponentQuaternion(Quaternion q, RotationAxis axis)
    {
        float theta;
        Quaternion axisRot;
        switch (axis)
        {
            case RotationAxis.X:
                theta = Mathf.Atan2(q.x, q.w);
                axisRot = new Quaternion(Mathf.Sin(theta), 0, 0, Mathf.Cos(theta));
                break;
            case RotationAxis.Y:
                theta = Mathf.Atan2(q.y, q.w);
                axisRot = new Quaternion(0, Mathf.Sin(theta), 0, Mathf.Cos(theta));
                break;
            // case RotationAxis.Z:
            default:
                theta = Mathf.Atan2(q.z, q.w);
                axisRot = new Quaternion(0, 0, Mathf.Sin(theta), Mathf.Cos(theta));
                break;
        }
        return axisRot;
    }
}
