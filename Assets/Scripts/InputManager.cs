using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> controller = new List<UnityEngine.XR.InputDevice>();
    
    void Start()
    {
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, controller);
    }
    
    void Update()
    {
        bool btn;
        if (controller.Count > 0 && controller[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out btn) && btn)
        {
            StateManager.Instance.GoToNextState();
        }
    }
}
