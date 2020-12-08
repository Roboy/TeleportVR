using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.G2OM;
using Widgets;

public class EyeGaze : MonoBehaviour, IGazeFocusable
{
    View view;

    /// <summary>
    /// Set reference to instance
    /// </summary>
    void Start()
    {
        view = this.GetComponentInParent<View>();
    }

    /// <summary>
    /// This method is called whenever the state if this object is looked at or not changes.
    /// Tell the connected view that it is now (not) being looked at by the operator.
    /// </summary>
    /// <param name="hasFocus">does the operator look at this object?</param>
    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            view.OnSelectionEnter();
        } else
        {
            view.OnSelectionExit();
        }
    }
}
