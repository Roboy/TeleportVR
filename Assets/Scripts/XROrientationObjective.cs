using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROrientationObjective : MonoBehaviour
{

    public bool trackOrientation = true;
    // Controller GameObject to take orientation from
    public Transform controller;
    // Objective Script controlling the hand orienation
    public BioIK.Orientation orientationObjective;

    public Transform target;

    // Derived by placing an empty GameObject at the writs position and
    // having the IK goal following it's rotation. 
    // By then applying rotations manually to this game object to visually align Roboy's
    // hand with the Oculus controllers we derived the following constants
    public Vector3 rotationOffset = new Vector3(-189.118f, -8.403992f, 15.2381f);

    //[Tooltip("Start for linear cutoff when error gets <= x the resulting weight is y")]
    //public Vector2 cutoffStart = new Vector2(10, 1);
    //[Tooltip("End of linear cutoff when error gets >= x with weight y")]
    //public Vector2 cutoffEnd = new Vector2(20, 0.1f);

    [SerializeField] private AnimationCurve cutoff;

    [SerializeField] private float errorR;
    [SerializeField] private float errorP;
    [SerializeField] private float weight;

    // Start is called before the first frame update
    void Start()
    {
        cutoff.preWrapMode = WrapMode.Clamp;
        cutoff.postWrapMode = WrapMode.Clamp;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion offset = Quaternion.Euler(rotationOffset);
        if (trackOrientation)
        {
            transform.rotation = controller.rotation * offset;

            errorR = Quaternion.Angle(target.rotation, transform.rotation);
            errorP = (target.position - controller.position).magnitude;
            errorP = 50* errorP;

            // linear falloff in cutoffStart <= error <= cutoffEnd
            float e = errorR + errorP;
            weight = cutoff.Evaluate(e);
            orientationObjective.SetWeight(weight);
        }
        else
        {
            orientationObjective.SetWeight(0);
        }
    }
}
