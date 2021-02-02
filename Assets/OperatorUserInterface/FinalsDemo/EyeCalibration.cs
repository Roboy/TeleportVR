using System;
using UnityEngine;
#if VIVESR
using ViveSR.anipal.Eye;
#endif

public class EyeCalibration : MonoBehaviour
{
    /// <summary>
    /// This method launches the eye-tracking calibration process of the Vive Pro Eye.
    /// This method can be hooked to a 3D Button
    /// </summary>
    public void Execute()
    {
#if VIVESR
        SRanipal_Eye_API.LaunchEyeCalibration(IntPtr.Zero);
#endif
    }
}
