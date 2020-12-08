using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Thi class sole purpose is to call the method AddEffectToChildren() on the curved UI on each LateUpdate, 
/// to curve all new generated widgets. The call seems to be efficient, so I seems to be an ok workaround. 
/// </summary>
public class ChildCurver : MonoBehaviour
{
    private CurvedUI.CurvedUISettings curvedUI;

    /// <summary>
    /// Get the reference to the CurvedUI.CurvedUISettings script
    /// </summary>
    void Start()
    {
        curvedUI = GetComponent<CurvedUI.CurvedUISettings>();
    }

    /// <summary>
    /// Curve all children of the curved UI
    /// </summary>
    void LateUpdate()
    {
        curvedUI.AddEffectToChildren();
    }
}
