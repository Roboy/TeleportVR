using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RudderPedals
{
    public class PresenceDetector : Singleton<PresenceDetector>
    {
        [System.Serializable]
        public class TrackerSwitcher
        {
            [Tooltip("CopyTransform script at SenseGlove root")]
            public JointTransfer.CopyTransfrom copyTransform;
            [Tooltip("XR input for the associated hand")]
            public Transform xrController;
            [Tooltip("Ghost hand, shown when paused")]
            public GameObject ghostHand;

            private Transform oldParent;
            private bool usingXR = false;

            internal void SwitchControllers()
            {
                if (usingXR)
                {
                    copyTransform.gameObject.transform.SetParent(oldParent);
                    copyTransform.position = true;
                    copyTransform.rotation = true;
                    usingXR = false;
                }
                else
                {
                    copyTransform.position = false;
                    copyTransform.rotation = false;
                    oldParent = copyTransform.gameObject.transform.parent;
                    copyTransform.gameObject.transform.SetParent(xrController, true);

                    usingXR = true;
                }
            }
        }

        [Tooltip("Time step to refresh presence detector in (seconds)")]
        public float presenceRefresh = 0.1f;
        public bool isPaused = false;

        public TrackerSwitcher leftGlove;
        public TrackerSwitcher rightGlove;

        private SerialReader pedalDetector;
        private bool oldLeft = false, oldRight = false;
        private bool oldMotorEnabled = false;
        private Timer animationTimer;

        // Start is called before the first frame update
        void Start()
        {
            pedalDetector = new SerialReader(refresh: presenceRefresh);
            StartCoroutine(pedalDetector.readAsyncContinously(OnUpdatePresence, OnError));
        }

        // Update is called once per frame
        void Update()
        {
        }

        private bool[] ParseData(string data)
        {
            try
            {
                if (data == null) return null;
                string[] args = data.Split(',');
                int leftInt = int.Parse(args[0]);
                int rightInt = int.Parse(args[1]);
                return new bool[] { leftInt != 0, rightInt != 0 };
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        private void OnUpdatePresence(string data)
        {
            bool[] lr = ParseData(data);
            if (lr == null)
            {
                return;
            }

            bool left = lr[0], right = lr[1];
            if ((!left || !right) && (oldLeft || oldRight))
            {
                Pause();
            }
            else if ((left && right) && isPaused)
            {
                Unpause();
            }

            oldLeft = left;
            oldRight = right;
        }

        public bool Pause()
        {
            if (isPaused)
            {
                return false;
            }

            Debug.Log("Paused");
            PauseMenu.Instance.show = true;
            isPaused = true;

            // Disable BioIK & wheelchair
            EnableControlManager.Instance.leftBioIKGroup.SetEnabled(false);
            EnableControlManager.Instance.rightBioIKGroup.SetEnabled(false);

            WheelchairStateManager.Instance.SetVisibility(true, StateManager.Instance.currentState == StateManager.States.HUD ? WheelchairStateManager.HUDAlpha : 1);

            PedalDriver.Instance.enabled = false;
            oldMotorEnabled = UnityAnimusClient.Instance.motorEnabled;
            //UnityAnimusClient.Instance.EnableMotor(false);

            AudioListener.pause = true;

            // switch gloves to paused mode
            leftGlove.SwitchControllers();
            leftGlove.ghostHand.SetActive(true);
            rightGlove.SwitchControllers();
            rightGlove.ghostHand.SetActive(true);
            return true;
        }

        public bool Unpause()
        {
            if (!isPaused)
            {
                return false;
            }

            Debug.Log("Unpaused");
            PauseMenu.Instance.show = false;
            isPaused = false;

            // Enable BioIK & wheelchair
            EnableControlManager.Instance.leftBioIKGroup.SetEnabled(true);
            EnableControlManager.Instance.rightBioIKGroup.SetEnabled(true);

            WheelchairStateManager.Instance.SetVisibility(StateManager.Instance.currentState != StateManager.States.HUD);

            //PedalDriver.Instance.enabled = true;
            //UnityAnimusClient.Instance.EnableMotor(oldMotorEnabled);
            
            AudioListener.pause = false;

            // switch gloves back to control mode
            leftGlove.SwitchControllers();
            leftGlove.ghostHand.SetActive(false);
            rightGlove.SwitchControllers();
            rightGlove.ghostHand.SetActive(false);
            return true;
        }

        private void OnError(string reason)
        {
            Debug.LogError(reason);
        }
    }
}
