using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pointer class to use the mouse for UI input
/// </summary>
public class PointerMouse : Pointer
{
    Camera cam;
    float scaling = 200.0f;

    /// <summary>
    /// Finds the mouse position in 2D screen coordinates and mocks a ray coming from roughly the headsets position through the mouse on the canvas.
    /// Then sends the controllers position and orientation to the UI Manager for pointing.
    /// </summary>
    public override void GetPointerPosition()
    {
        //Debug.Log(Input.mousePosition.x);
        PushPointerPosition(cam.transform.position + new Vector3(0.1f, 0.1f, 0.1f), 
            cam.transform.rotation.eulerAngles + new Vector3((-Input.mousePosition.y + (Screen.width*2.1f)) * scaling / Screen.width, (Input.mousePosition.x - (Screen.width/2)) * scaling / Screen.width, 0));
    }

    /// <summary>
    /// Init
    /// </summary>
    public override void SubclassStart()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// Update gets mouse pointer position every frame
    /// </summary>
    public void Update()
    {
        GetPointerPosition();        
    }
}
