using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public List<UnityEngine.XR.InputDevice> controllerLeft = new List<UnityEngine.XR.InputDevice>();
    public List<UnityEngine.XR.InputDevice> controllerRight = new List<UnityEngine.XR.InputDevice>();

    private bool lastMenuBtn;
    private bool lastGrabLeft;
    private bool lastGrabRight;

    void Start()
    {
        GetLeftController();
        GetRightController();
    }

    /// try to get the left controller, if possible.<!-- return if the controller can be referenced.-->
    public bool GetLeftController()
    {
        if (controllerLeft.Count == 0)
        {
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, controllerLeft);
        }
        return controllerLeft.Count > 0;
    }

    /// try to get the right controller, if possible.<!-- return if the controller can be referenced.-->
    public bool GetRightController()
    {
        if (controllerRight.Count == 0)
        {
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, controllerRight);
        }
        return controllerRight.Count > 0;
    }

    void FixedUpdate()
    {
        if (!Widgets.WidgetInteraction.settingsAreActive)
        {
            bool btn;
            if (GetLeftController())
            {
                // Go to the other mode
                if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out btn) && btn && !lastMenuBtn)
                {
                    StateManager.Instance.GoToNextState();
                }
                lastMenuBtn = btn;

                /*if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) && btn && !lastGrabLeft)
                {
                    Widgets.WidgetInteraction.Instance.ToggleRightBody();
                }
                lastGrabLeft = btn;*/

                // update the emotion buttons
                if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out btn))
                {
                    UnityAnimusClient.Instance.LeftButton1 = btn;
                }
                if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out btn))
                {
                    UnityAnimusClient.Instance.LeftButton2 = btn;
                }

                // recalibrate the roboybody
                if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out btn) && btn)
                {
                    RecalibrateUpperBody.Instance.Calibrate();
                    print("Calibrated...");
                }

                // drive the wheelchair
                if (StateManager.Instance.currentState == StateManager.States.Construct)
                {
                    Vector2 joystick;
                    if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystick)) {
                        // move forward or backwards
                        DifferentialDriveControl.Instance.V_L = 0.1f * joystick.y;
                        DifferentialDriveControl.Instance.V_R = 0.1f * joystick.y;

                        //rotate
                        if (joystick.x > 0)
                        {
                            DifferentialDriveControl.Instance.V_R -= 0.05f * joystick.x;
                        }
                        else
                        {
                            DifferentialDriveControl.Instance.V_L += 0.05f * joystick.x;
                        }
                    }
                }
            }
            else
            {
                if (UnityAnimusClient.Instance != null)
                {
                    UnityAnimusClient.Instance.LeftButton1 = Input.GetKeyDown(KeyCode.F);
                    UnityAnimusClient.Instance.LeftButton2 = Input.GetKeyDown(KeyCode.R);
                }
            }
            if (GetRightController())
            {
                /*if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) && btn && !lastGrabRight)
                {
                    Widgets.WidgetInteraction.Instance.ToggleLeftBody();
                }
                lastGrabRight = btn;*/

                // update the emotion buttons
                if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out btn))
                {
                    UnityAnimusClient.Instance.RightButton1 = btn;
                }
                if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out btn))
                {
                    UnityAnimusClient.Instance.RightButton2 = btn;
                }
            }
            else
            {
                if (UnityAnimusClient.Instance != null)
                {
                    UnityAnimusClient.Instance.RightButton1 = Input.GetKeyDown(KeyCode.G);
                    UnityAnimusClient.Instance.RightButton2 = Input.GetKeyDown(KeyCode.T);
                }
            }

        }

        /*if (Input.GetKeyDown(KeyCode.F))
        {
            Widgets.WidgetInteraction.Instance.ToggleRightBody();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Widgets.WidgetInteraction.Instance.ToggleLeftBody();
        }*/
    }
}
