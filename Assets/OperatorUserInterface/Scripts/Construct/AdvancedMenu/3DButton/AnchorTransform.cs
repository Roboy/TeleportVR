using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorTransform : MonoBehaviour
{
    public GameObject[] buttons;

    public bool ActivateScript;
    public Transform FollowObject;
    SpringJoint spring;
    Vector3 defaultLocalPos;
    Vector3 oldPos;
    Quaternion oldRot;

    /// <summary>
    /// Set reference to instance,
    /// store default position
    /// </summary>
    void Start()
    {
        spring = this.GetComponent<SpringJoint>();
        if(ActivateScript && FollowObject == null)
        {
            Debug.LogError("FollowObject not specified.");
        }
        oldPos = FollowObject.position;
        defaultLocalPos = this.transform.localPosition;
        ResetAnchor();
    }

    /// <summary>
    /// Update the anchor position according to FollowObject's movement.
    /// Change the spring mass when the anchor position changes in order to prevent bouncing.
    /// </summary>
    void FixedUpdate()
    {
        if (ActivateScript)
        {
            bool posChanged = !FollowObject.position.Equals(oldPos);
            bool rotChanged = !FollowObject.rotation.Equals(oldRot);
            if (!posChanged && !rotChanged)
            {
                spring.massScale = 10f;
            }
            else
            {
                if (rotChanged)
                {
                    spring.massScale = 0.00001f;
                    Quaternion rotDiff = FollowObject.rotation * Quaternion.Inverse(oldRot);
                    spring.connectedAnchor = oldPos + rotDiff * (spring.connectedAnchor - oldPos);
                }
                if (posChanged)
                {
                    spring.massScale = 0.00001f;
                    spring.connectedAnchor = spring.connectedAnchor + (FollowObject.position - oldPos);
                }
            }
            oldRot = FollowObject.rotation;
            oldPos = FollowObject.position;
        }
    }

    /// <summary>
    /// Set the anchor position to the stored default position.
    /// That means the anchor should have the same local offset to the frame as it has at start.
    /// </summary>
    public void ResetAnchor()
    {
        this.transform.localPosition = defaultLocalPos;
        spring.connectedAnchor = this.transform.position;
    }
}
