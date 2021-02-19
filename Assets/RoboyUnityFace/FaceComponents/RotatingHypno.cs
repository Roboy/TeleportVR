using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingHypno : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 thePosition = transform.TransformPoint(0, 0, 0);
        transform.RotateAround(thePosition, Vector3.back, 110 * Time.deltaTime);
    }
}
