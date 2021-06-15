using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script moves the associated transfrom such that the reference eye transform matches the goal. 
/// Calibration is done calibrationTime number of seconds after the game has started, to insure the operator
/// has enough time to settle.
/// </summary>
public class StartCalibrator : MonoBehaviour
{
    [Tooltip("Operator eye transfrom, used as calibration reference")]
    public Transform eye;
    [Tooltip("Goal eye transform after calibration")]
    public Transform goal;
    
    [Tooltip("Number of seconds after game start, calibration should occurr (seconds)")]
    public float calibrationTime = 2;
    private bool calibrated = false;

    void Update()
    {
        if (!calibrated && Time.time >= calibrationTime)
        {

            // Change transfrom position
            Vector3 move = goal.position - eye.position;
            transform.position += move;

            calibrated = true;
            Debug.Log($"Calibration done, moved: {move}");
        }
    }
}
