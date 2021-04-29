using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoboyFK
{
    public class FKHingeJoint : MonoBehaviour
    {

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
