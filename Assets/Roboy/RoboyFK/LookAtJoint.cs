using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboyFK
{
    public class LookAtJoint : MonoBehaviour
    {
        public Transform controller;
        public BioIK.BioJoint joint;

        private Quaternion initialRotation;
        private float initialTarget;

        // Start is called before the first frame update
        void Start()
        {
            initialRotation = transform.localRotation;
            initialTarget = (float)joint.X.GetTargetValue();
        }

        // Update is called once per frame
        void Update()
        {

            Quaternion rotation = Quaternion.LookRotation(controller.position - transform.position, -controller.forward);
            // offset rotation by 90° in x
            Quaternion localRotation = Quaternion.Inverse(transform.parent.rotation) * rotation;
            localRotation *= Quaternion.Euler(90f, 0f, 0f);

            float angle = Quaternion.Angle(localRotation, initialRotation);

            Quaternion component = Util.ComponentQuaternion(localRotation, RotationAxis.Z);
            float componentAngle = Quaternion.Angle(component, initialRotation);

            // Debug.Log($"Quaternion angle: {angle}, Component angle: {componentAngle}");
            joint.X.TargetValue = Mathf.Min(
                Mathf.Max((float)joint.X.LowerLimit, componentAngle),
                (float)joint.X.UpperLimit);
        }
    }

}
