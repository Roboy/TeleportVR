using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class transforms the associated game object into a space suitable for using it as 
/// a BioIk objective. 
/// Additionally, it externally computes the orientation deviation
/// between a controller and the resulting BioIK.Orientation transform. This deviation is used to
/// limit the rotation Objective's weight, giving priority to the position contraint when orientation 
/// is in unreachable configurations.
/// </summary>
public class XROffset : MonoBehaviour
{

    [System.Serializable]
    public class Position
    {
        [Tooltip("If position offset should be applyed in the negative y-direction of controller up")]
        public bool enabled = true;
        [Tooltip("Up direction distance from the controller to the operators hand (m)")]
        public float offset = .1f;
        [Tooltip("Adjusted controller coordinate system")]
        public Transform controllerUp;
    }

    [System.Serializable]
    public class Orientation
    {
        [Tooltip("If rotation offset should be applyed")]
        public bool enabled = true;
        [Tooltip("BioIK orientation objective to be observed")]
        public BioIK.Orientation objective;
        [Tooltip("Resulting transform of the orientation objective")]
        public Transform target;
        [Tooltip("Mapping from target deviation (degrees) to orientation objective weight")]
        public AnimationCurve cutoff;

    }

    [Tooltip("If the current script references a right hand")]
    public bool isRight = false;
    [Tooltip("Transform of the tracked controller")]
    public Transform controller;
    public Position position;
    public Orientation orientation;

    [Header("Read only values")]
    [Tooltip("Current orientation offset, overwritten at startup")]
    public Vector3 orientationOffset;
    [Tooltip("Computed error values")]
    [SerializeField] float errorR, errorP, weight;

    // official Oculus Quest 1 attachement (left hand rotation)
    private readonly Vector3 SGQuest1Offset = new Vector3(-212f, 0f, 90f);

    void Awake()
    {
        orientation.cutoff.preWrapMode = WrapMode.Clamp;
        orientation.cutoff.postWrapMode = WrapMode.Clamp;
#if SENSEGLOVE
        Vector3 o = SGQuest1Offset;
        // Since the rotation offset is wrt. the left hand, we mirror to get the one for the right
        orientationOffset = isRight ? new Vector3(o.x, -o.y, -o.z) : new Vector3(o.x, o.y, o.z);
        position.enabled = true;
#else
        orientationOffset = new Vector3(-189.118f, -8.403992f, 15.2381f);
        // Don't offset position when using oculus controllers
        position.enabled = false;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = Vector3.zero;
        if (position.enabled)
        {
            offset = -position.controllerUp.up * position.offset;
        }
        transform.position = controller.position + offset;

        if (orientation.enabled)
        {
            transform.rotation = controller.rotation * Quaternion.Euler(orientationOffset);

            float errorR = Quaternion.Angle(orientation.target.rotation, transform.rotation);
            float errorP = (orientation.target.position - controller.position).magnitude - position.offset;
            errorP = Mathf.Max(50 * errorP, 0);

            // linear falloff in cutoffStart <= error <= cutoffEnd
            float weight = orientation.cutoff.Evaluate(errorR + errorP);
            orientation.objective.SetWeight(weight);
            // publish internal values 
            this.errorR = errorR;
            this.errorP = errorP;
            this.weight = weight;
        }
        else
        {
            orientation.objective.SetWeight(0);
        }
    }
}
