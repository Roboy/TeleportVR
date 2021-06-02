using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseGloveDebugger : MonoBehaviour
{

    public GameObject[] leftHandJoints;
    public GameObject[] rightHandJoints;
    private float scale = 0.01f;
    private float targetFrame = 5;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var joints in new GameObject[][] { leftHandJoints, rightHandJoints })
        {
            foreach (var joint in joints)
            {
                var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.transform.position = Vector3.zero;
                obj.transform.localScale = new Vector3(scale, scale, scale);
                obj.transform.SetParent(joint.transform, false);
            }
        }
    }
}
