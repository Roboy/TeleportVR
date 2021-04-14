using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This script allows to move the displays if the widget for that is currently active.
/// </summary>
public class DisplayMover : MonoBehaviour
{
    [SerializeField] private bool isLeft;
    private float timer;

    List<UnityEngine.XR.InputDevice> controller = new List<UnityEngine.XR.InputDevice>();

    private float _speed = 0.2f;
    private readonly Dictionary<KeyCode, Vector3> _moveDictLeft = new Dictionary<KeyCode, Vector3>
    {
        {KeyCode.Q, Vector3.down},
        {KeyCode.W, Vector3.forward},
        {KeyCode.E, Vector3.up},
        {KeyCode.A, Vector3.left},
        {KeyCode.S, Vector3.back},
        {KeyCode.D, Vector3.right},
    };

    private readonly Dictionary<KeyCode, Vector3> _moveDictRight = new Dictionary<KeyCode, Vector3>
    {
        {KeyCode.U, Vector3.down},
        {KeyCode.I, Vector3.forward},
        {KeyCode.O, Vector3.up},
        {KeyCode.J, Vector3.left},
        {KeyCode.K, Vector3.back},
        {KeyCode.L, Vector3.right},
    };

    // A reference to the keys under which the display offsets get saved between sessions
    public static readonly string LeftDisplayOffsetKey = "LeftDisplayOffset";
    public static readonly string RightDisplayOffsetKey = "RightDisplayOffset";

    // The path under which the offset values of the displays are stored
    private string _displayOffsetKey;

    /// <summary>
    /// Read out the offset values of the displays from the last time, if they exist on this device
    /// </summary>
    private void Start()
    {
        UnityEngine.XR.InputDeviceCharacteristics role = isLeft ?
            UnityEngine.XR.InputDeviceCharacteristics.Left : UnityEngine.XR.InputDeviceCharacteristics.Right;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(role, controller);

        _displayOffsetKey = isLeft ? LeftDisplayOffsetKey : RightDisplayOffsetKey;
        transform.localPosition = new Vector3(  PlayerPrefs.GetFloat(_displayOffsetKey + "X", 0),
                                                PlayerPrefs.GetFloat(_displayOffsetKey + "Y", -2),
                                                PlayerPrefs.GetFloat(_displayOffsetKey + "Z", 6));
    }

    /// <summary>
    /// Update the display positions.
    /// </summary>
    void Update()
    {
        if (controller.Count > 0)
        {
            if (Widgets.WidgetInteraction.settingsAreActive)
            {
                // use the joystick to move the displays horizontally or vertically
                Vector2 axisValue;
                if (controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out axisValue) &&
                    axisValue.magnitude > 0)
                {
                    transform.localPosition += Time.deltaTime * _speed * axisValue.x * Vector3.right;
                    transform.localPosition += Time.deltaTime * _speed * axisValue.y * Vector3.up;
                }

                // use the buttons to move the displays in forward or backwards (zoom in or out)
                bool btn;
                if (controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out btn) && btn)
                {
                    transform.localPosition += Time.deltaTime * _speed * Vector3.forward;
                }

                if (controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out btn) && btn)
                {
                    transform.localPosition += Time.deltaTime * _speed * Vector3.back;
                }
            }
        }
        else
        {
            // if no controllers are connected, allow the keyboard mock
            UnityEngine.XR.InputDeviceCharacteristics role = isLeft ?
            UnityEngine.XR.InputDeviceCharacteristics.Left : UnityEngine.XR.InputDeviceCharacteristics.Right;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(role, controller);

            if (Widgets.WidgetInteraction.settingsAreActive)
            {
                Dictionary<KeyCode, Vector3> moveDict = isLeft ? _moveDictLeft : _moveDictRight;

                foreach (KeyCode key in moveDict.Keys)
                {
                    // TODO bring it back!
                    //if (Input.GetKey(key))
                    //{
                    //    transform.localPosition += Time.deltaTime * _speed * moveDict[key];
                    //}
                }
            }
        }

        // save the offset between sessions every 0.2s
        if (Widgets.WidgetInteraction.settingsAreActive)
        {
            if (timer >= 0.2)
            {
                timer = 0;
                SaveOffsets();
            }
            timer += Time.deltaTime;
        }
    }

    // Save the offsets to the PlayerPrefs
    private void SaveOffsets() {
        PlayerPrefs.SetFloat(_displayOffsetKey + "X", transform.localPosition.x);
        PlayerPrefs.SetFloat(_displayOffsetKey + "Y", transform.localPosition.y);
        PlayerPrefs.SetFloat(_displayOffsetKey + "Z", transform.localPosition.z);
    }
}

// my preferred offset for Roboy: 
// Left: -0.0530509, 0.0192513, 2, Scale: 0.6
// Right:0.1860066, -0.15494, 2, Scale: 0.6
