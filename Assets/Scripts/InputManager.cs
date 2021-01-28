using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> controllerLeft = new List<UnityEngine.XR.InputDevice>();
    List<UnityEngine.XR.InputDevice> controllerRight = new List<UnityEngine.XR.InputDevice>();
    
    void Start()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, controllerLeft);
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, controllerRight);
    }
    
    void Update()
    {
        bool btn;
        if (controllerLeft.Count > 0) {
            if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out btn) && btn)
            {
                StateManager.Instance.GoToNextState();
            }
            if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) && btn)
            {
                Widgets.WidgetInteraction.Instance.ToggleRightBody();
            }
            if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) && btn)
            {
                Widgets.WidgetInteraction.Instance.ToggleLeftBody();
            }
        }
        else {
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, controllerLeft);
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, controllerRight);

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
}
