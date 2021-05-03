using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoboyFK
{
    public class FKHingeJoint : MonoBehaviour
    {
        [Tooltip("Controlling rotation")]
        public Transform controller;
        [Tooltip("Joint to write the output angle to")]
        public BioIK.BioJoint joint;
        [Tooltip("If the output angle should be computed by the direct angle between the controller's rotation and this objects rotation, " +
            "or if the axis-wise angle computation should be used")]
        public bool approximative = false;
        [Tooltip("Rotation axis to track from the controller")]
        public RotationAxis axis = RotationAxis.Z;
        [Tooltip("If the output angle should be inverted")]
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
                Quaternion axisRot = Util.ComponentQuaternion(q, axis);
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

    }

}
