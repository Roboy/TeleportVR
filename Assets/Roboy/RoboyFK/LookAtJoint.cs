using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoboyFK
{
    public class LookAtJoint : MonoBehaviour
    {
        public Transform controller;
        public BioIK.BioJoint joint;
        public bool isRight = true;


        private Quaternion initialRotation;

        // Start is called before the first frame update
        void Start()
        {
            initialRotation = transform.localRotation;
        }

        // Update is called once per frame
        void Update()
        {

            Vector3 projectionNormal = joint.X.Axis;
            projectionNormal = transform.rotation * projectionNormal;

            Plane projectionPlane = new Plane(projectionNormal, transform.position);
            Vector3 dir = controller.position - transform.position;
            Vector3 rayDir = Vector3.Dot(dir, projectionNormal) > 0 ? -projectionNormal : projectionNormal;
            Ray intersectionRay = new Ray(controller.position, rayDir);
            float dist;
            if (projectionPlane.Raycast(intersectionRay, out dist))
            {
                Vector3 projected = controller.position + rayDir * dist;
                Debug.DrawRay(projected, projected - transform.position, Color.yellow);
                Quaternion rotation = Quaternion.LookRotation(projected - transform.position,
                    isRight ? projectionNormal : -projectionNormal);
                Quaternion localRotation = Quaternion.Inverse(transform.parent.rotation) * rotation;

                // offset rotation by 90° in x
                localRotation *= Quaternion.Euler(90f, 0, 0);
                float angle = Quaternion.Angle(localRotation, initialRotation);
                joint.X.TargetValue = Mathf.Min(Mathf.Max((float)joint.X.LowerLimit, angle), (float)joint.X.UpperLimit);
            }
        }
    }

}
