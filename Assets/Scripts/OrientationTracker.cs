using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationTracker : MonoBehaviour
{

    public Transform objectToTrack;
    public Vector3 axis;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        var orn = objectToTrack.transform.rotation.eulerAngles;
        orn -= new Vector3(0,-270.0f,0);
        transform.rotation = Quaternion.Euler(orn.z, orn.y, -orn.x);
        Debug.Log(transform.rotation.eulerAngles);

    }

    //// Update is called once per frame
    //void Update()
    //{
    //    var orn = objectToTrack.transform.rotation.eulerAngles;
    //    orn -= correction(axis);
    //    orn = Vector3.Scale(orn, axis);
    //    //Debug.Log(orn);
    //    //var newQ =
    //    transform.rotation = Quaternion.Euler(orn.x, orn.y, orn.z);
    //    //transform.rotation = Quaternion.Euler(ClipAngle(orn.x), ClipAngle(orn.y), ClipAngle(orn.z));// new Quaternion(newQ.x, newQ.y, newQ.z, newQ.w);
    //    //transform.rotation.Set(newQ.x, newQ.y, newQ.z, newQ.w);
    //    Debug.Log(transform.rotation.eulerAngles);

    //}

    Vector3 correction(Vector3 axis)
    {
        if (axis.Equals(Vector3.up))
        {
            return new Vector3(0, -270.0f, 0);
        }
        else if (axis.Equals(Vector3.forward))
        {
            return new Vector3(0, 0, 0);
        }
        else if (axis.Equals(Vector3.right))
        {
            return new Vector3(0, 0, 0);
        }
        return Vector3.zero;
    }

    public float ClipAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }
        return angle;
    }
}
