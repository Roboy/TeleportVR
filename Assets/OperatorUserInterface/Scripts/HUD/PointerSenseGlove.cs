using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pointer used with the SenseGlove, where a ray is coming from the gloves position and orientation.
/// </summary>
public class PointerSenseGlove : Pointer
{
    private SenseGlove_Teleport teleport;

    /// <summary>
    /// Send the ray the sense glove is pointing at.
    /// </summary>
    public override void GetPointerPosition()
    {
        PushPointerPosition(teleport.pointerOriginZ.position, teleport.pointerOriginZ.rotation.eulerAngles);
    }

    /// <summary>
    /// At the start, this method sets the reference to the first found SenseGlove_Teleport script.
    /// </summary>
    public override void SubclassStart()
    {
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(SenseGlove_Teleport));
        if (objects.Length > 0)
        {
            teleport = (SenseGlove_Teleport) objects[0];
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
