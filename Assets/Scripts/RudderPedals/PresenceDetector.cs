using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RudderPedals
{

    public class PresenceDetector : MonoBehaviour
    {
        public float refresh = 0.1f;
        public bool isPaused = false;


        private SerialReader pedalDetector;
        private bool oldLeft = false, oldRight = false;


        // Start is called before the first frame update

        void Start()
        {
            pedalDetector = new SerialReader(refresh: refresh);
            StartCoroutine(pedalDetector.readAsyncContinously(onUpdate, onError));
        }

        // Update is called once per frame
        void Update()
        {

        }

        private bool[] parseData(string data)
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

        private void onUpdate(string data)
        {
            bool[] lr = parseData(data);
            if (lr == null)
            {
                return;
            }

            bool left = lr[0], right = lr[1];
            if ((!left || !right) && (oldLeft || oldRight))
            {
                isPaused = true;
                Debug.Log("Paused");
                Time.timeScale = 0;
                AudioListener.pause = true;

                PauseMenu.Instance.show = 1;
            }
            else if ((left && right) && isPaused)
            {
                Debug.Log("Unpaused");
                Time.timeScale = 1;
                AudioListener.pause = false;

                PauseMenu.Instance.show = 0;
            }

            oldLeft = left;
            oldRight = right;
        }


        private void onError(string reason)
        {
            Debug.LogError(reason);
        }


    }
}
