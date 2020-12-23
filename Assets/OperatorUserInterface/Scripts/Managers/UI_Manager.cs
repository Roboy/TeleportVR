using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the raycast input system with different input methods and also with the Curved UI
/// </summary>
public class UI_Manager : MonoBehaviour
{
    [Header("Select Interaction Techniques")]
    [Tooltip("Technique for pointing.")]
    public PointerTechnique pointerTechnique;

    private Camera cam;
    private AudioSource clickSound;
    private RaycastManager raycastManager;

    private Pointer pointer;

    private Vector2 pointerPos;

    public enum PointerTechnique { PointerMouse, PointerViveController, PointerSenseGlove, PointerController };

    #region Setup

    public void Start()
    {
        CreatePointer();
        cam = Camera.main;
        clickSound = this.GetComponent<AudioSource>();
        raycastManager = this.GetComponent<RaycastManager>();
    }

    #endregion

    /// <summary>
    /// Gets the position and rotation of the pointer and passes it to the curved UI input module
    /// </summary>
    /// <param name="position">position of pointer</param>
    /// <param name="rotation">rotation of pointer</param>
    public void Point(Vector3 position, Vector3 rotation)
    {
        this.transform.position = position;
        this.transform.rotation = Quaternion.Euler(rotation);

        // Send pointer information to Curved UI Input Module to use event system
        CurvedUIInputModule.CustomControllerRay = new Ray(this.transform.position, this.transform.forward);

        // this draws the ray - the rest of the raycast Manager is not being used
        raycastManager.GetRaycastHit(position, rotation);
    }


    /// <summary>
    /// Creates Pointer script for the pointer technique selected.
    /// </summary>
    private void CreatePointer()
    {
        switch (pointerTechnique)
        {
            case PointerTechnique.PointerMouse:
                pointer = this.gameObject.AddComponent<PointerMouse>();
                return;                
            case PointerTechnique.PointerSenseGlove:
                pointer = this.gameObject.AddComponent<PointerSenseGlove>();
                break;
            case PointerTechnique.PointerController:
                pointer = this.gameObject.AddComponent<PointerController>();
                break;
            default:
                throw new System.Exception("No pointer technique specified.");
        }
    }
}
