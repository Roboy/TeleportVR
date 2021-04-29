using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboyFK
{
    public class Util
    {

        // Computes the per-axis contribution of q.
        // Based on: https://stackoverflow.com/questions/43606135/split-quaternion-into-axis-rotations
        public static Quaternion ComponentQuaternion(Quaternion q, RotationAxis axis)
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
    public enum RotationAxis
    {
        X, Y, Z
    }


}
