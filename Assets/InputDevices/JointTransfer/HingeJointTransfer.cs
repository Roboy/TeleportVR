using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BioIK;
using System;
#if SENSEGLOVE
using SG;
#endif


namespace JointTransfer
{
    /// <summary>
    /// This copies the rotation in a single axis from a controller to a target joint.
    /// It's main use is in transferring arbitrary joint rotation form to a hinge joint like, BioIk Joint (finger joints).
    /// </summary>
    public class HingeJointTransfer : MonoBehaviour
    {
        [Tooltip("Controlling rotation")]
        public Transform controller;
        [Tooltip("Joint to write the output angle to")]
        public BioJoint joint;
        // Axix wise computation is more accurate, but can sometimes suffer from inaccuracies, as the 
        // quaternion decomposition underlying it is not perfect.
        [Tooltip("If the output angle should be computed by the direct angle between the controller's rotation and this objects rotation, " +
            "or if the axis-wise angle computation should be used")]
        public bool approximative = false;
        [Tooltip("Rotation axis to track from the controller")]
        public RotationAxis axis = RotationAxis.Z;
        [Tooltip("If the output angle should be inverted")]
        public bool invert = false;

        public AngleRangeStore angleRangeStore;

#if SENSEGLOVE
        public SG_SenseGloveHardware hand;
#endif
        public float angle;
        public float input, minAngle, maxAngle;


        public float initialRotation;
        public float initialTarget;
        public bool initialized = false;

#if SENSEGLOVE
        // Update is called once per frame
        void Update()
        {
            if (hand.IsLinked && !initialized)
            {
                StartCoroutine(WaitAndInit(0.5f));
            }
            if (!initialized)
            {
                return;
            }

            BioJoint.Motion motion = joint.X;
            input = Utils.RemapAngle(getRotationOnAxis(controller.localRotation, axis));
            angle = input;

            if (angleRangeStore.minRotation.ContainsKey(controller.name)
                && angleRangeStore.maxRotation.ContainsKey(controller.name))
            {
                minAngle = getRotationOnAxis(angleRangeStore.minRotation[controller.name], axis);
                maxAngle = getRotationOnAxis(angleRangeStore.maxRotation[controller.name], axis);
                angle = Remap(angle,
                    invert ? maxAngle : minAngle,
                    invert ? minAngle : maxAngle,
                    (float)motion.LowerLimit,
                    (float)motion.UpperLimit);
            }
            else
            {
                angle = invert ? -angle : angle;
            }

            joint.X.SetTargetValue(Mathf.Clamp(angle, (float)motion.LowerLimit, (float)motion.UpperLimit));
        }

        private static float getRotationOnAxis(Quaternion rot, RotationAxis axis)
        {
            return axis switch
            {
                RotationAxis.X => rot.eulerAngles.x,
                RotationAxis.Y => rot.eulerAngles.y,
                RotationAxis.Z => rot.eulerAngles.z,
                _ => float.NaN,
            };
        }
        private static float getRotationOnAxis(Vector3 rot, RotationAxis axis)
        {
            return axis switch
            {
                RotationAxis.X => rot.x,
                RotationAxis.Y => rot.y,
                RotationAxis.Z => rot.z,
                _ => float.NaN,
            };
        }

        private IEnumerator WaitAndInit(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            initialRotation = Utils.RemapAngle(getRotationOnAxis(controller.localRotation, axis));
            initialTarget = (float)joint.X.GetTargetValue();
            initialized = true;
            yield return true;
        }

        private float Remap(float x, float minIn, float maxIn, float minOut, float maxOut)
        {
            if (Mathf.Approximately(maxIn - minIn, 0))
            {
                return (minOut + maxOut) / 2;
            }
            return ((x - minIn) / (maxIn - minIn)) * maxOut + minOut;
        }
#endif
    }
}

