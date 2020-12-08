using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for pointers, that give a position and rotation for a raycast
/// </summary>
public abstract class Pointer : MonoBehaviour
{
    private UI_Manager UI_Manager;

    /// <summary>
    /// Starts the pointer object
    /// </summary>
    void Start()
    {
        UI_Manager = GetComponent<UI_Manager>();
        SubclassStart();
    }

    /// <summary>
    /// Helper for inheriting classes to have their own start
    /// </summary>
    public abstract void SubclassStart();
    
    /// <summary>
    /// Returns the pointers position in 2D screen coordinates (pixels) 
    /// </summary>
    public abstract void GetPointerPosition();
    
    /// <summary>
    /// Pushes pointer position to UI_Manager every Update() call 
    /// </summary>
    /// <param name="position">pointers position</param>
    /// <param name="rotation">pointers rotation</param>
    public void PushPointerPosition(Vector3 position, Vector3 rotation)
    {
        UI_Manager.Point(position, rotation);
    }
}
