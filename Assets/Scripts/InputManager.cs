using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Widgets;

public class InputManager : Singleton<InputManager>
{
    public List<UnityEngine.XR.InputDevice> controllerLeft = new List<UnityEngine.XR.InputDevice>();
    public List<UnityEngine.XR.InputDevice> controllerRight = new List<UnityEngine.XR.InputDevice>();
    public HandController handController;

    [SerializeField] VRGestureRecognizer vrGestureRecognizer;

    private bool lastMenuBtn;
    private bool lastGrabLeft;
    private bool lastGrabRight;
    bool nodded, waiting;

    void Start()
    {
        handController.Init();
        GetLeftController();
        GetRightController();

        vrGestureRecognizer.Nodded += OnNodded;
        vrGestureRecognizer.HeadShaken += OnHeadShaken;
    }

    private bool GetControllerAvailable(bool leftController)
    {
        return leftController ? GetLeftControllerAvailable() : GetRightControllerAvailable();
    }

    public InputDevice GetController(bool leftController)
    {
        return leftController ? controllerLeft[0] : controllerRight[0];
    }

    /// try to get the left controller, if possible.<!-- return if the controller can be referenced.-->
    public bool GetLeftControllerAvailable()
    {
        if (controllerLeft.Count == 0)
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, controllerLeft);
        }
        return controllerLeft.Count > 0;
    }

    /// try to get the right controller, if possible.<!-- return if the controller can be referenced.-->
    public bool GetRightControllerAvailable()
    {
        if (controllerRight.Count == 0)
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, controllerRight);
        }
        return controllerRight.Count > 0;
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

    public bool GetControllerBtn(InputFeatureUsage<bool> inputFeature, bool leftController)
    {
        if (GetControllerAvailable(leftController))
        {
            if (GetController(leftController).TryGetFeatureValue(inputFeature, out var btn))
            {
                return btn;
            }
        }

        return false;
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

    /// <summary>
    /// Handle input from the controllers.
    /// </summary>
    void Update()
    {
        handController.GetMotorPositions();
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

                // update the emotion buttons
                if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out btn))
                {
                    UnityAnimusClient.Instance.LeftButton1 = btn;
                }
                if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out btn))
                {
                    UnityAnimusClient.Instance.LeftButton2 = btn;
                }

                if (Training.TutorialSteps.Instance != null &&
                    StateManager.Instance.currentState == StateManager.States.Training)
                {
                    // check if the arm is grabbing 
                    if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.LEFT_HAND)
                    {
                        if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.NextStep(praise: true);
                        }

                        if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                           btn)
                        {
                            Training.TutorialSteps.Instance.CorrectUser("grip");
                        }
                    }

                    if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.RIGHT_HAND)
                    {
                        if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.NextStep(praise: true);
                        }
                        if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.CorrectUser("grip");
                        }
                    }

                    // check left arm
                    if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.LEFT_ARM)
                    {
                        if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.CorrectUser("index");
                        }
                    }

                    //// check right arm
                    if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.RIGHT_ARM)
                    {

                        //if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                        //    btn)
                        //{
                        //    Training.TutorialSteps.Instance.NextStep(praise: true);
                        //}

                        if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out btn) &&
                            btn)
                        {
                            Training.TutorialSteps.Instance.CorrectUser("index");
                        }
                    }

                    //if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.RIGHT_HAND)
                    //{
                    //    if (controllerRight[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                    //        btn)
                    //    {
                    //        Training.TutorialSteps.Instance.CorrectUser("grip");
                    //    }
                    //}

                    //if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.LEFT_HAND)
                    //{
                    //    if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out btn) &&
                    //        btn)
                    //    {
                    //        Training.TutorialSteps.Instance.CorrectUser("grip");
                    //    }
                    //}
                }

                // drive the wheelchair
                if (//StateManager.Instance.currentState == StateManager.States.Construct || 
                    StateManager.Instance.currentState != StateManager.States.HUD)
                {
                    Vector2 joystick;
                    if (controllerLeft[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out joystick))
                    {
                        bool wheelchairIsActive = joystick.sqrMagnitude > 0.01f;

                        // Show that the wheelchair is active in the state manager
                        WidgetInteraction.SetBodyPartActive(56, wheelchairIsActive);

                        if (wheelchairIsActive)
                        {
                            if (StateManager.Instance.currentState == StateManager.States.Training &&
                                Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.WHEELCHAIR)
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
                else
                {
                    // Show that the wheelchair is active in the state manager
                    WidgetInteraction.SetBodyPartActive(56, false);
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
                    if (Training.TutorialSteps.Instance.currentStep == Training.TutorialSteps.TrainingStep.HEAD)
                    {
                        if (!waiting & Training.TutorialSteps.Instance.waitingForNod) StartCoroutine(WaitForNod());
                        // if (nodded)


                    }
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
    }

    [System.Serializable]
    public class HandController
    {
        public BioIK.BioIK rightHand;
        public BioIK.BioIK leftHand;
        public int minMotorStep = 0;
        public int maxMotorStep = 800;

        private Dictionary<string, double> minAngles = new Dictionary<string, double>();
        private Dictionary<string, double> maxAngles = new Dictionary<string, double>();

        public void Init()
        {
            List<BioIK.BioIK> hands = new List<BioIK.BioIK>() { leftHand, rightHand };
            foreach (var hand in hands)
            {
                foreach (var segment in hand.Segments)
                {
                    if (segment.Joint == null) continue;
                    minAngles[segment.name] = segment.Joint.X.LowerLimit;
                    maxAngles[segment.name] = segment.Joint.X.UpperLimit;
                }
            }

        }

        // rotations interface: 
        // Right hand: 
        // rh_FFJ3 [0..800]
        // rh_MFJ3 [0..800]
        // rh_RFJ3 [0..800]
        // rh_THJ4 [0..800]
        //
        // Left hand:
        // lh_FFJ3 [0..800]
        // lh_MFJ3 [0..800]
        // lh_RFJ3 [0..800]
        // lh_THJ4 [0..800]
        public List<float> GetMotorPositions()
        {
            List<List<string>> jointSets = new List<List<string>>()
            {
                new List<string>() {"rh_FFJ3", "rh_FFJ2"},
                new List<string>() {"rh_MFJ3", "rh_MFJ2"},
                new List<string>() {"rh_RFJ3", "rh_RFJ2"},
                new List<string>() {"rh_THJ4"},
                new List<string>() {"lh_FFJ3", "lh_FFJ2"},
                new List<string>() {"lh_MFJ3", "lh_MFJ2"},
                new List<string>() {"lh_RFJ3", "lh_RFJ2"},
                new List<string>() {"lh_THJ4"},

            };

            Dictionary<string, double> currentAngles = new Dictionary<string, double>();
            foreach (var hand in new List<BioIK.BioIK>() { rightHand, leftHand })
            {
                foreach (var segment in hand.Segments)
                {
                    if (segment.Joint == null) continue;
                    currentAngles[segment.name] = segment.Joint.X.CurrentValue;
                }
            }

            List<float> motorPos = new List<float>();
            foreach (var joints in jointSets)
            {
                List<double> min = new List<double>();
                List<double> max = new List<double>();
                List<double> current = new List<double>();
                foreach (var name in joints)
                {
                    min.Add(minAngles[name]);
                    max.Add(maxAngles[name]);
                    current.Add(currentAngles[name]);
                }

                double range = L2Distance(min, max);
                double dist = L2Distance(min, current);
                motorPos.Add((int)(minMotorStep + (maxMotorStep - minMotorStep) * dist / range));
            }
            Debug.Log("motorPos = " + string.Join(", ",
                motorPos
                .ConvertAll(i => i.ToString())
                .ToArray()));
            return motorPos;
        }

        private double L2Distance(List<double> a, List<double> b)
        {
            if (a.Count != b.Count)
            {
                return double.NegativeInfinity;
            }
            double dist = 0;
            for (int i = 0; i < a.Count; i++)
            {
                dist += (a[i] - b[i]) * (a[i] - b[i]);
            }
            return dist / a.Count;
        }


    }
}

