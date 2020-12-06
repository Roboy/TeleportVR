using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTorsoRotation : MonoBehaviour
{
    /// <summary>
    /// This script has not been implemented/integrated completely, because the KatVR hardware got a defect in the process.
    /// The intention was to write a script, that applies the operator torso's movement and rotation to the construct main menu.
    /// The movement values (position) of the operator can be retrieved from the CameraOrigin object.
    /// The rotation values of the operator can be retrieved from the Walker object.
    /// </summary>

    Transform torso;

    /// <summary>
    /// Set reference to instance
    /// </summary>
    void Start()
    {
        torso = GameObject.FindGameObjectWithTag("KatVRWalker").transform;
    }

    /// <summary>
    /// Apply operator torso's rotation smoothly to the menu.
    /// </summary>
    void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, torso.rotation, Time.deltaTime * 100);
    }
}
