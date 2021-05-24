using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCalibrator : MonoBehaviour
{

    public Transform eye;
    public Transform goal;

    public float calibrationTime = 2;
    // two state variables are used to prevent a potential overflow
    private bool calibrated = false;

    // Update is called once per frame
    void Update()
    {
        if (!calibrated && Time.time >= calibrationTime)
        {
            // rotation
            Vector3 oldPos = transform.position;
            transform.position = goal.position;
            Quaternion offsetRotation = Quaternion.FromToRotation(eye.forward, goal.forward);
            transform.rotation = offsetRotation * transform.rotation;
            transform.position = oldPos;

            // position
            Vector3 move = goal.position - eye.position;
            transform.position += move;

            calibrated = true;
            Debug.Log($"Calibration done, rotated by: {offsetRotation}, moved by: {move}");
        }
        else if (!calibrated)
        {
        }
    }
}
