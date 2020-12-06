using System;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyeCalibration : MonoBehaviour
{
    /// <summary>
    /// This method launches the eye-tracking calibration process of the Vive Pro Eye.
    /// This method can be hooked to a 3D Button
    /// </summary>
    public void Execute()
    {
        SRanipal_Eye_API.LaunchEyeCalibration(IntPtr.Zero);
    }
}
