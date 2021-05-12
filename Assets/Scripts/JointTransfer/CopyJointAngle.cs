using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JointTransfer
{
    public class CopyJointAngle : MonoBehaviour
    {
        public BioIK.BioJoint from;
        public BioIK.BioJoint to;
        public RotationAxis axis;

        private BioIK.BioJoint.Motion fromMotion;
        private BioIK.BioJoint.Motion toMotion;

        // Start is called before the first frame update
        void Start()
        {
            switch (axis)
            {
                case RotationAxis.X:
                    fromMotion = from.X;
                    toMotion = to.X;
                    break;
                case RotationAxis.Y:
                    fromMotion = from.Y;
                    toMotion = to.Y;
                    break;
                case RotationAxis.Z:
                    fromMotion = from.Z;
                    toMotion = to.Z;
                    break;
                default: break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // copies the target value from the given from Axis to the target Axis
            toMotion.TargetValue = fromMotion.TargetValue;
        }
    }

}
