using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public List<UnityEngine.XR.InputDevice> controllerLeft = new List<UnityEngine.XR.InputDevice>();
    public List<UnityEngine.XR.InputDevice> controllerRight = new List<UnityEngine.XR.InputDevice>();

    [SerializeField] VRGestureRecognizer vrGestureRecognizer;

    private bool lastMenuBtn;
    private bool lastGrabLeft;
    private bool lastGrabRight;
    bool nodded, waiting;

    void Start()
    {
        GetLeftController();
        GetRightController();

        vrGestureRecognizer.Nodded += OnNodded;
        vrGestureRecognizer.HeadShaken += OnHeadShaken;
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

    void OnNodded()
    {
        nodded = true;
        Debug.LogError("Yes");
    }

    void OnHeadShaken()
    {
        Debug.LogError("no");
    }

    IEnumerator WaitForNod()
    {
        Debug.Log("waiting for a nod");
        waiting = true;
        nodded = false;
        yield return new WaitUntil(() => nodded);
        waiting = false;
        {
            Debug.Log("moving on");
            Training.TutorialSteps.Instance.NextStep();
        }
        Debug.Log("user confirmed");
    }

    void FixedUpdate()
    {
        if (StateManager.Instance.currentState == StateManager.States.HUD)
            UnityAnimusClient.Instance.EnableMotor(true);
        else
            UnityAnimusClient.Instance.EnableMotor(false);
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
                
                if ( //StateManager.Instance.currentState == StateManager.States.Construct || 
                    Training.TutorialSteps.Instance != null &&
                    StateManager.Instance.currentState == StateManager.States.Training)
                {
                    // check if the arm is grabbing 
                    if (Training.TutorialSteps.Instance.currentStep == 10)
                    {
                        if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.NextStep();
                        }
                    }
                    
                    // check if the left arm is moving 
                    if (Training.TutorialSteps.Instance.currentStep == 2)
                    {
                        
                        if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                            btn)
                        {
                           
                            Training.TutorialSteps.Instance.NextStep(praise: true);
                        }

                        if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton , out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.CorrectUser();
                        }
                    }

                    // check if the right arm is moving 
                    if (Training.TutorialSteps.Instance.currentStep == 3)
                    {
                        
                        if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.NextStep(praise: true);
                        }

                        if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.CorrectUser();
                        }
                    }
                }

                // drive the wheelchair
                if (//StateManager.Instance.currentState == StateManager.States.Construct || 
                    StateManager.Instance.currentState != StateManager.States.HUD)
                {
                    Vector2 joystick;
                    if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystick))
                    {
                        print(StateManager.Instance);
                        print(Training.TutorialSteps.Instance);
                        if (StateManager.Instance.currentState == StateManager.States.Training && Training.TutorialSteps.Instance.currentStep == 5)
                        {
                            if (joystick.sqrMagnitude > 0.1f)
                            {
                                Training.TutorialSteps.Instance.NextStep();
                            }
                        }
                        
                        float speed = 0.05f;
                        // move forward or backwards
                        DifferentialDriveControl.Instance.V_L = speed * joystick.y;
                        DifferentialDriveControl.Instance.V_R = speed * joystick.y;

                        //rotate
                        if (joystick.x > 0)
                        {
                            DifferentialDriveControl.Instance.V_R -= 0.5f * speed * joystick.x;
                        }
                        else
                        {
                            DifferentialDriveControl.Instance.V_L += 0.5f * speed * joystick.x;
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
                
                if ( //StateManager.Instance.currentState == StateManager.States.Construct || 
                    StateManager.Instance.currentState == StateManager.States.Training)
                {
                    if (Training.TutorialSteps.Instance.currentStep == 1)
                    {
                        if (!waiting) StartCoroutine(WaitForNod());
                       // if (nodded)
                       
                       
                    }
                    
                    // check if the arm is grabbing 
                    //if (Training.TutorialSteps.Instance.currentStep == 5)
                    //{
                    //    if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                    //        btn)
                    //    {
                    //        Training.TutorialSteps.Instance.NextStep();
                    //    }
                    //}
                    
                    // check if the arm is grabbing 
                    //if (Training.TutorialSteps.Instance.currentStep == 3)
                    //{
                    //    if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                    //        btn)
                    //    {
                    //        Training.TutorialSteps.Instance.NextStep();
                    //    }
                    //}
                }

                if ( //StateManager.Instance.currentState == StateManager.States.Construct || 
                    StateManager.Instance.currentState == StateManager.States.Training)
                {
                    // check if the arm is grabbing 
                    //if (Training.TutorialSteps.Instance.currentStep == 2)
                    //{
                    //    if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                    //        btn)
                    //    {
                    //        Training.TutorialSteps.Instance.NextStep();
                    //    }
                    //}
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
