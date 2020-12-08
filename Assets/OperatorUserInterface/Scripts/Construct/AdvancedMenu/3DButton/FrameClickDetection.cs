using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FrameClickDetection : MonoBehaviour
{

    #region properties
    public bool isToggle;
    public UnityEvent[] onPress;
    public UnityEvent[] onUnpress;
    private int onPressIndex;
    private int onUnpressIndex;
    private bool wait;
    private Collider pressurePlateCollider;
    public Transform pressurePlateTransform;
    private MeshRenderer meshRenderer;
    public Color defaultColor;
    private Color red;
    private bool toggle;
    #endregion

    /// <summary>
    /// Initialize variables, dependent on consistent prefab hierarchy.
    /// </summary>
    private void Start()
    {
        wait = false;
        toggle = false;
        onPressIndex = 0;
        onUnpressIndex = 0;
        //pressurePlateTransform = transform.parent.GetChild(0);
        pressurePlateCollider = pressurePlateTransform.GetComponent<Collider>();
        defaultColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.GetColor("_Color");

        red = new Color();
        red.a = 1.0f;
        red.b = 0f;
        red.g = 0.63f;
        red.r = 1f;
    }

    /// <summary>
    /// Checks if the PressurePlate goes through the frame.
    /// If "wait" is set to true, then the button is already pressed.
    /// Otherwise the button gets activated.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (pressurePlateCollider.Equals(other))
        {
            if (!wait)
            { 
                press();
                highlightOn();
            }
        }
    }

    /// <summary>
    /// When the PressurePlate exits the frame collider, its direction is checked.
    /// It is either pushed further in (-> the button remains pressed)
    /// or it is on its way back to its default position (-> the button gets released).
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (pressurePlateCollider.Equals(other))
        {
            Debug.Log("Frame Exit");
            if (transform.InverseTransformPoint(pressurePlateTransform.position).z < transform.localPosition.z)
            {
                wait = false;
                unpress();
                highlightOff();
            }
            else
            {
                wait = true;
            }
        }
    }

    /// <summary>
    /// Implement all functionality when the button is pressed here.
    /// Default: it executes the actions defined in onPress chronologically.
    /// </summary>
    void press()
    {
        if(onPress.Length > 0)
        {
            onPress[onPressIndex].Invoke();
            onPressIndex++;
            if (onPressIndex.Equals(onPress.Length))
            {
                onPressIndex = 0;
            }
        }
    }

    /// <summary>
    /// Implement all functionality when the button is unpressed here.
    /// Default: it executes the actions defined in onUnpress chronologically.
    /// </summary>
    void unpress()
    {
        if (onUnpress.Length > 0)
        {
            onUnpress[onUnpressIndex].Invoke();
            onUnpressIndex++;
            if (onUnpressIndex.Equals(onUnpress.Length))
            {
                onUnpressIndex = 0;
            }
        }
    }

    /// <summary>
    /// Highlights the Button when pressed as feedback for the user.
    /// Default: The frame changes its color to light blue.
    /// </summary>
    void highlightOn()
    {
        foreach(MeshRenderer childMeshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
        {
            childMeshRenderer.material.SetColor("_Color", red);
        }
    }

    /// <summary>
    /// Turn off highlight for the button when it is released.
    /// If this instance is a toggle button, then the button's frame can turn red as visual feedback for the user that the toggle is active.
    /// </summary>
    public void highlightOff()
    {
        if (isToggle)
        {
            if (toggle)
            {
                foreach (MeshRenderer childMeshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
                {
                    childMeshRenderer.material.SetColor("_Color", defaultColor);
                }
                toggle = false;
            }
            else
            {
                foreach (MeshRenderer childMeshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
                {
                    childMeshRenderer.material.SetColor("_Color", Color.red);
                }
                toggle = true;
            }
        }
        else
        {
            foreach (MeshRenderer childMeshRenderer in transform.GetComponentsInChildren<MeshRenderer>())
            {
                childMeshRenderer.material.SetColor("_Color", defaultColor);
            }
        }
    }
}
