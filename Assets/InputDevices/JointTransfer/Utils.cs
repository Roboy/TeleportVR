using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JointTransfer
{
    public class Utils
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

        public static IEnumerable<GameObject> GetChildren(Transform parent, System.Predicate<GameObject> predicate)
        {
            foreach (Transform c in parent)
            {
                if (predicate(c.gameObject))
                {
                    yield return c.gameObject;
                    foreach (var tmp in GetChildren(c, predicate))
                    {
                        yield return tmp;
                    }
                }
            }
        }

        public static float RemapAngle(float angle)
        {
            return angle > 180 ? angle - 360 : angle;
        }

        public static Vector3 RemapRotationVector3(Vector3 angles)
        {
            return new Vector3(RemapAngle(angles.x), RemapAngle(angles.y), RemapAngle(angles.z));
        }
    }

    public enum RotationAxis
    {
        X, Y, Z
    }
}
