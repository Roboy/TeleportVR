using BioIK;
using System.Collections.Generic;
using UnityEngine;

public class EnableControlManager : MonoBehaviour
{
    public BioSegment left_hand;
    public BioSegment right_hand;

    List<ControllerStruct> controllers = new List<ControllerStruct>();
    struct ControllerStruct
    {
        public BioSegment segment;
        public UnityEngine.XR.InputDevice controller;
        bool enabled;

        public ControllerStruct(BioSegment _segment, UnityEngine.XR.InputDevice _inputDevice)
        {
            segment = _segment;
            controller = _inputDevice;
            enabled = false;
        }

        public void SetEnabled(bool _enabled)
        {
            enabled = _enabled;
            if (enabled)
            {
                controller.SendHapticImpulse(0, 0.005f, 0.01f);
            }
            else
            {
                controller.StopHaptics();
            }

            for (int i = 0; i < segment.Objectives.Length; i++)
            {
                segment.Objectives[i].enabled = enabled;
            }
        }

        public bool IsEnabled()
        {
            return enabled;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        if (InputManager.Instance.GetLeftController())
        {
            controllers.Add(new ControllerStruct(left_hand, InputManager.Instance.controllerLeft[0]));
        }

        if (InputManager.Instance.GetRightController())
        {
            controllers.Add(new ControllerStruct(right_hand, InputManager.Instance.controllerRight[0]));
        }
    }


    // Update is called once per frame
    void Update()
    {
        for (int i=0;i<controllers.Count;i++)
        {
            var device = controllers[i];
            var enabled = false;
            if (device.controller.isValid) {
                device.controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out enabled);
            }
            device.SetEnabled(enabled);

        }
    }


}
