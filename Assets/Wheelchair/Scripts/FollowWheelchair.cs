using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWheelchair : MonoBehaviour
{

    public Transform tfWheelchair;
    public Vector3 offset;

    void Update()
    {
        // It's camera position
        transform.position = tfWheelchair.position + offset;
    }
}
