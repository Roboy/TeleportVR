using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorFpsLimiter : MonoBehaviour
{
    /// <summary>
    /// Limit the fps in the editor to 72 (fps of the quest) to reduce computer load
    /// </summary>
    void Awake () {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0; // VSync must be disabled.
        Application.targetFrameRate = 72;
        print("Limited fps to 72");
#endif
    }
}
