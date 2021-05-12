using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JointTransfer
{

    /// <summary>
    /// Given the position of a controller, this class computes the
    /// a look rotation aiming at the controller position,
    /// but constrained to only the target BioIK joint's axis of rotation.
    /// It's used for controlling single axis of rotation thumb joints.
    /// </summary>
    public class LookAtTransfer : MonoBehaviour
    {
        public Transform controller;
        public BioIK.BioJoint joint;
        public bool isRight = true;

        public RotationAxis axis = RotationAxis.X;

        private Quaternion initialRotation;
        private BioIK.BioJoint.Motion axisMotion;

        // Start is called before the first frame update
        void Start()
        {
            initialRotation = transform.localRotation;
            switch (axis)
            {
                case RotationAxis.X:
                    axisMotion = joint.X;
                    break;
                case RotationAxis.Y:
                    axisMotion = joint.Y;
                    break;
                case RotationAxis.Z:
                    axisMotion = joint.Z;
                    break;
                default: break;
            }
        }

        // Update is called once per frame
        void Update()
        {

            // This projects controller.position to the plane normal to the joint's
            // axis of rotation, determined by this.axis.
            // From this projected position, the required angle to point
            // in the direction of this position from transform.position is computed
            // and set as the joint's axis target value (axisMotion)
            Vector3 projectionNormal = axisMotion.Axis;
            projectionNormal = transform.rotation * projectionNormal;

            Plane projectionPlane = new Plane(projectionNormal, transform.position);
            Vector3 dir = controller.position - transform.position;
            Vector3 rayDir = Vector3.Dot(dir, projectionNormal) > 0 ? -projectionNormal : projectionNormal;
            Ray intersectionRay = new Ray(controller.position, rayDir);

            if (projectionPlane.Raycast(intersectionRay, out float dist))
            {
                Vector3 projected = controller.position + rayDir * dist;
                Quaternion rotation = Quaternion.LookRotation(projected - transform.position, isRight ? projectionNormal : -projectionNormal);
                Quaternion localRotation = Quaternion.Inverse(transform.parent.rotation) * rotation;

                // offset rotation by 90° in x
                localRotation *= Quaternion.Euler(90f, 0, 0);
                float angle = Quaternion.Angle(localRotation, initialRotation);
                // clamp angle to joint ranges
                axisMotion.TargetValue = Mathf.Clamp(angle, (float)axisMotion.LowerLimit, (float)axisMotion.UpperLimit);
            }
        }
    }

}
