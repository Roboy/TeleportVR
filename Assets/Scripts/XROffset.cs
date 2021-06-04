using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROffset : MonoBehaviour
{

    [System.Serializable]
    public class Position
    {
        public bool enabled = true;
        [Tooltip("Up direction distance from the controller to the operators hand (m)")]
        public float offset = .1f;
    }

    [System.Serializable]
    public class Orientation
    {
        public bool enabled = true;
        public BioIK.Orientation objective;
        public Transform target;
        public Transform controllerUp;
        public AnimationCurve cutoff;

        [Header("Read only values")]
        public Vector3 orientationOffset;
        public float errorR, errorP, weight;
    }
    public bool isRight = false;
    public Transform controller;
    public Position position;
    public Orientation orientation;

    // official Oculus Quest 1 attachement (left hand rotation)
    private readonly Vector3 SGQuest1Offset = new Vector3(-212f, 0f, 90f);

    // Start is called before the first frame update
    void Start()
    {
        orientation.cutoff.preWrapMode = WrapMode.Clamp;
        orientation.cutoff.postWrapMode = WrapMode.Clamp;
#if SENSEGLOVE
        Vector3 o = SGQuest1Offset;
        // invert rotation x-Axis
        orientation.orientationOffset = isRight ? new Vector3(o.x, -o.y, -o.z) : new Vector3(o.x, o.y, o.z);
#else
        orientation.orientationOffset = new Vector3(-189.118f, -8.403992f, 15.2381f);
#endif
    }

    // Update is called once per frame
    void Update()
    {

        if (position.enabled)
        {
            transform.position = controller.position - orientation.controllerUp.up * position.offset;
        }

        if (orientation.enabled)
        {
            transform.rotation = controller.rotation * Quaternion.Euler(orientation.orientationOffset);

            float errorR = Quaternion.Angle(orientation.target.rotation, transform.rotation);
            float errorP = (orientation.target.position - controller.position).magnitude - position.offset;
            errorP = Mathf.Max(50 * errorP, 0);

            // linear falloff in cutoffStart <= error <= cutoffEnd
            float weight = orientation.cutoff.Evaluate(errorR + errorP);
            orientation.objective.SetWeight(weight);
            // publish internal values 
            orientation.errorR = errorR;
            orientation.errorP = errorP;
            orientation.weight = weight;
        }
        else
        {
            orientation.objective.SetWeight(0);
        }
    }
}
