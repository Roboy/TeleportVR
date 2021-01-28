using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> controllerLeft = new List<UnityEngine.XR.InputDevice>();
    List<UnityEngine.XR.InputDevice> controllerRight = new List<UnityEngine.XR.InputDevice>();

    private bool lastMenuBtn;
    private bool lastGrabLeft;
    private bool lastGrabRight;
    
    void Start()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, controllerLeft);
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, controllerRight);
    }
    
    void Update()
    {
        bool btn;
        if (controllerLeft.Count > 0) {
            if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out btn) && btn && !lastMenuBtn)
            {
                StateManager.Instance.GoToNextState();
            }
            lastMenuBtn = btn;
                
            if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) && btn && !lastGrabLeft)
            {
                Widgets.WidgetInteraction.Instance.ToggleRightBody();
            }
            lastGrabLeft = btn;
        }
        else {
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, controllerLeft);
        }
        if (controllerRight.Count > 0) {
            if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) && btn && !lastGrabRight)
            {
                Widgets.WidgetInteraction.Instance.ToggleLeftBody();
            }
            lastGrabRight = btn;
        }
        else {
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, controllerRight);
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Widgets.WidgetInteraction.Instance.ToggleRightBody();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Widgets.WidgetInteraction.Instance.ToggleLeftBody();
        }
    }
}
