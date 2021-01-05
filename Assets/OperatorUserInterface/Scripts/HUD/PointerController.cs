using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// A pointer used with the SenseGlove, where a ray is coming from the gloves position and orientation.
/// </summary>
public class PointerController : Pointer
{
    private XRBaseController teleport;

    /// <summary>
    /// Send the ray the sense glove is pointing at.
    /// </summary>
    public override void GetPointerPosition()
    {
        //PushPointerPosition(teleport.pointerOriginZ.position, teleport.pointerOriginZ.rotation.eulerAngles);
        PushPointerPosition(teleport.transform.position, teleport.transform.eulerAngles);
    }

    /// <summary>
    /// At the start, this method sets the reference to the first found SenseGlove_Teleport script.
    /// </summary>
    public override void SubclassStart()
    {
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(XRBaseController));
        print("objects.Length" + objects.Length);
        if (objects.Length > 0)
        {
            teleport = (XRBaseController) objects[0];
        }
    }

    /// <summary>
    /// Update the ray the sense glove is pointing at.
    /// </summary>
    void Update()
    {
        GetPointerPosition();
    }
}
