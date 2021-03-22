using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorFpsLimiter : MonoBehaviour
{
    void Awake () {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0; // VSync must be disabled.
        Application.targetFrameRate = 72;
        print("Limited fps to 72");
#endif
    }
}
