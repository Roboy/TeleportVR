using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisplayMover : MonoBehaviour
{
    [SerializeField] private bool isLeft;
    private float timer;

    List<UnityEngine.XR.InputDevice> controller = new List<UnityEngine.XR.InputDevice>();
    //[SerializeField] private UnityEngine.XR.InputDevice controller;

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

    private string DisplayOffsetKey;

    private void Start()
    {
        //UnityEngine.XR.InputDeviceRole role = isLeft ?
        //    UnityEngine.XR.InputDeviceRole.LeftHanded : UnityEngine.XR.InputDeviceRole.RightHanded;
        UnityEngine.XR.InputDeviceCharacteristics role = isLeft ?
            UnityEngine.XR.InputDeviceCharacteristics.Left : UnityEngine.XR.InputDeviceCharacteristics.Right;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(role, controller);

        DisplayOffsetKey = isLeft ? LeftDisplayOffsetKey : RightDisplayOffsetKey;
        transform.localPosition = new Vector3(  PlayerPrefs.GetFloat(DisplayOffsetKey + "X", 0),
                                                PlayerPrefs.GetFloat(DisplayOffsetKey + "Y", 0),
                                                PlayerPrefs.GetFloat(DisplayOffsetKey + "Z", 6));
    }

    void Update()
    {
        //print("Controller len: " + controller.Count);
        if (controller.Count > 0)
        {
            if (Widgets.WidgetInteraction.settingsAreActive)
            {
                Vector2 axisValue;
                if (controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out axisValue) &&
                    axisValue.magnitude > 0)
                {
                    //print("Axis: " + axisValue);
                    transform.localPosition += Time.deltaTime * _speed * axisValue.x * Vector3.right;
                    transform.localPosition += Time.deltaTime * _speed * axisValue.y * Vector3.up;
                }

                bool btn;
                if (controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out btn) && btn)
                {
                    transform.localPosition += Time.deltaTime * _speed * Vector3.forward;
                }

                //print("btn: " + btn);

                if (controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out btn) && btn)
                {
                    //Debug.Log("btn: " + axisValue);
                    transform.localPosition += Time.deltaTime * _speed * Vector3.back;
                }
            }
        }
        else
        {
            UnityEngine.XR.InputDeviceCharacteristics role = isLeft ?
            UnityEngine.XR.InputDeviceCharacteristics.Left : UnityEngine.XR.InputDeviceCharacteristics.Right;
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(role, controller);

            if (Widgets.WidgetInteraction.settingsAreActive)
            {
                Dictionary<KeyCode, Vector3> moveDict = isLeft ? _moveDictLeft : _moveDictRight;

                foreach (KeyCode key in moveDict.Keys)
                {
                    if (Input.GetKey(key))
                    {
                        transform.localPosition += Time.deltaTime * _speed * moveDict[key];
                    }
                }
            }
        }

        if (Widgets.WidgetInteraction.settingsAreActive)
        {
            if (timer >= 0.2)
            {
                timer = 0;
                string txt = "Display left:" + isLeft.ToString() + ", " + (1000000 * transform.localPosition).ToString();
                print(txt);
                SaveOffsets();
            }
            timer += Time.deltaTime;
        }
    }

    private void SaveOffsets() {
        PlayerPrefs.SetFloat(DisplayOffsetKey + "X", transform.localPosition.x);
        PlayerPrefs.SetFloat(DisplayOffsetKey + "Y", transform.localPosition.y);
        PlayerPrefs.SetFloat(DisplayOffsetKey + "Z", transform.localPosition.z);
    }
}

// my preferred offset for Roboy: 
// Left: -0.0530509, 0.0192513, 2, Scale: 0.6
// Right:0.1860066, -0.15494, 2, Scale: 0.6
