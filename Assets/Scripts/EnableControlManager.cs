using BioIK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Widgets;

public class EnableControlManager : MonoBehaviour
{
    public BioSegment left_hand;
    public BioSegment right_hand;
    public BioIK.BioIK left_fingers;
    public BioIK.BioIK right_fingers;

    List<ControllerStruct> controllers = new List<ControllerStruct>();
    struct ControllerStruct
    {
        public BioSegment hand_segment;
        public BioIK.BioIK hand_body;
        public UnityEngine.XR.InputDevice controller;
        bool enabled;

        public ControllerStruct(BioSegment _segment, BioIK.BioIK _body, UnityEngine.XR.InputDevice _inputDevice)
        {
            hand_segment = _segment;
            hand_body = _body;
            controller = _inputDevice;
            enabled = false;
        }

        public void SetController(InputDevice newController)
        {
            controller = newController;
        }

        public void SetEnabled(bool _enabled)
        {
            enabled = _enabled;
            if (enabled)
            {
                controller.SendHapticImpulse(0, 0.005f);
            }
            else
            {
                controller.StopHaptics();
            }

            for (int i = 0; i < hand_segment.Objectives.Length; i++)
            {
                hand_segment.Objectives[i].enabled = enabled;
            }
        }

        public bool IsEnabled()
        {
            return enabled;
        }

        public void UpdateFingers(double value)
        {
            foreach (var segment in hand_body.Segments)
            {
                if (segment.Joint != null)
                {
                    //if (segment.Joint.name.Contains("TH") && !segment.Joint.name.Contains("J1"))
                    //{

                    //}
                    if((segment.Joint.name.Contains("TH") && !segment.Joint.name.Contains("J5") ) || // && !segment.Joint.name.Contains("J2")) ||
                        (segment.Joint.name.Contains("LF") && !segment.Joint.name.Contains("J5")) ||// && !segment.Joint.name.Contains("J4")) ||
                        (!segment.Joint.name.Contains("J4")))
                    {
                        var range = segment.Joint.X.UpperLimit - segment.Joint.X.LowerLimit;

                        segment.Joint.X.SetTargetValue(value * range - segment.Joint.X.LowerLimit);
                    }
                    
                }
                    

            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (InputManager.Instance.GetLeftController())
        {
            controllers.Add(new ControllerStruct(left_hand, left_fingers, InputManager.Instance.controllerLeft[0]));
        }

        if (InputManager.Instance.GetRightController())
        {
            controllers.Add(new ControllerStruct(right_hand, right_fingers, InputManager.Instance.controllerRight[0]));
        }
    }

   


        // Update is called once per frame
        void Update()
    {
        if (controllers.Count < 2)
        {
            controllers.Clear();
            if (InputManager.Instance.GetLeftController())
            {
                controllers.Add(new ControllerStruct(left_hand, left_fingers, InputManager.Instance.controllerLeft[0]));
            }

            if (InputManager.Instance.GetRightController())
            {
                controllers.Add(new ControllerStruct(right_hand, right_fingers, InputManager.Instance.controllerRight[0]));
            }
        }
        //controllers[0].SetController(InputManager.Instance.controllerLeft[0]);
        //controllers[1].SetController(InputManager.Instance.controllerRight[0]);
        
        for (int i=0;i<controllers.Count;i++)
        {
            var device = controllers[i];
            var _enabled = false;
            float trigger = 0.0f;
            if (device.controller.isValid) {
                device.controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out _enabled);
                device.controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out trigger);
            }
            
            // Show that the arm is active in the state manager
            WidgetInteraction.SetBodyPartActive(53 - i, _enabled);

            // Show that the fingers are active in the state manager
            WidgetInteraction.SetBodyPartActive(55 - i, trigger > 0.05f);
            
            device.SetEnabled(_enabled);
            device.UpdateFingers(trigger);

        }
    }


}
