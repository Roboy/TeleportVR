using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ButtonRigidbodyConstraint : MonoBehaviour
{
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    /// <summary>
    /// Safe default position and rotation.
    /// </summary>
    private void Start()
    {
        defaultPos = this.transform.localPosition;
        defaultRot = this.transform.localRotation;
    }

    /// <summary>
    /// Fix the rotation and position of this object, except movement along the local z-axis (forward-backward).
    /// </summary>
    private void Update()
    {
        this.transform.localRotation = defaultRot;
        this.transform.localPosition = new Vector3(defaultPos.x, defaultPos.y, this.transform.localPosition.z);
    }

    /// <summary>
    /// Reset this object's position and rotation to the default values.
    /// </summary>
    public void InitialState()
    {
        this.transform.localRotation = defaultRot;
        this.transform.localPosition = defaultPos;
    }
}
