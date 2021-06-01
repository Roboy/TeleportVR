using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROffset : MonoBehaviour
{

    [System.Serializable]
    public class Position
    {
        public bool enabled = true;
        public float offset = 0f;
    }

    [System.Serializable]
    public class Orientation
    {
        public bool enabled = true;
        public BioIK.Orientation objective;
        public Transform target;
        public Vector3 orientationOffset;
        public AnimationCurve cutoff;
        public float errorR, errorP, weight;
    }

    public Transform controller;
    public Position position;
    public Orientation orientation;

    // Start is called before the first frame update
    void Start()
    {
        orientation.cutoff.preWrapMode = WrapMode.Clamp;
        orientation.cutoff.postWrapMode = WrapMode.Clamp;
#if SENSEGLOVE
        orientation.orientationOffset = new Vector3(-189.118f, -8.403992f, 15.2381f);
#else
        orientation.orientationOffset = new Vector3(-189.118f, -8.403992f, 15.2381f);
#endif
    }

    // Update is called once per frame
    void Update()
    {

        if (position.enabled)
        {
            transform.position = controller.position - controller.up * position.offset;
        }

        if (orientation.enabled)
        {
            transform.rotation = controller.rotation * Quaternion.Euler(orientation.orientationOffset);

            float errorR = Quaternion.Angle(orientation.target.rotation, transform.rotation);
            float errorP = (orientation.target.position - controller.position).magnitude;
            errorP = 50 * errorP;

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
