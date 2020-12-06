using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class follows the rotation of the main camera with a slerp instead of a direct copy of the rotation. 
/// It will start the slerp, if the rotational distance between the own rotation and the cameras rotation is bigger than a threshold angle (start slerp angle) and stops the slerp, if the distance is smaller than a second threshold (stop slerp angle).
/// The threshold is bigger, if the menu is pointed at.
/// </summary>
public class SlerpToCamera : MonoBehaviour
{
    // for timing the slerp
    private float startTime;

    // slerp state
    private bool slerp = false;

    [Tooltip("Speed to slerp back to camera rotation.")]    
    public float slerpSpeed = 10f;

    [Header("Thresholds if pointed")]
    [Tooltip("Threshold to start slerp if pointed.")]
    public float startSlerpAnglePointed = 15f;
    [Tooltip("Threshold to stop slerp if pointed.")]
    public float stopSlerpAnglePointed = 4f;

    [Header("Thresholds if not pointed")]
    [Tooltip("Threshold to start slerp if not pointed.")]
    public float startSlerpAngleNotPointed = 4f;
    [Tooltip("Threshold to stop slerp if not pointed.")]
    public float stopSlerpAngleNotPointed = 1f;

    // Right now only one menu, so ill only look for bottom menu pointed
    [Tooltip("Pull the pointed attribute from menu.")]
    //public Menu bottomMenu;
    
    // These angles are used as thresholds and exchanged according to the pointed attribute
    private float flexibleStartSlerpAngle;
    private float flexibleStopSlerpAngle;

    void Start()
    {
        flexibleStartSlerpAngle = startSlerpAngleNotPointed;
        flexibleStopSlerpAngle = stopSlerpAngleNotPointed;
    }

    /// <summary>
    /// Here the correct thresholds are set according to the pointed attribute
    /// </summary>
    void Update()
    {
        /*
        if (bottomMenu.GetPointed())
        {
            flexibleStartSlerpAngle = startSlerpAnglePointed;
            flexibleStopSlerpAngle = stopSlerpAnglePointed;
        }
        else
        {
        */
            flexibleStartSlerpAngle = startSlerpAngleNotPointed;
            flexibleStopSlerpAngle = stopSlerpAngleNotPointed;
        /*
        }
        */

        // copy translation
        this.transform.position = Camera.main.transform.position;

        // follow rotation
        CorrectRotation();        
    }

    /// <summary>
    /// Handles the start and stop of slerp according to thresholds and slerps
    /// </summary>
    void CorrectRotation()
    {
        // Calc rotational distance
        float rotationalDistance = Quaternion.Angle(this.transform.rotation, Camera.main.transform.rotation);
        
        // start slerp
        if (rotationalDistance >= flexibleStartSlerpAngle && slerp == false)
        {
            slerp = true;
            startTime = Time.time;
            // Debug.Log("Start Slerp");
        }

        // stop slerp
        else if (rotationalDistance <= flexibleStopSlerpAngle && slerp == true)
        {
            slerp = false;
            // Debug.Log("Stop Slerp");
        }

        // If slerp is active, use Quaternion.Slerp to correct rotation
        if (slerp)
        {
            float slerpProgress = (Time.time - startTime) / slerpSpeed;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Camera.main.transform.rotation, slerpProgress);
        }
    }
}
