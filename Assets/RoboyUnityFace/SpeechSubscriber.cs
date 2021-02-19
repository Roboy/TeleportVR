using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class SpeechSubscriber : UnitySubscriber<MessageTypes.RoboyCognition.SpeechSynthesis>
    {
        private RoboyAnimator animator;
        private bool isMessageReceived;
        private bool talking;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            animator = GetComponent<RoboyAnimator>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.RoboyCognition.SpeechSynthesis message)
        {
            talking = "sil" != message.phoneme;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            if (isMessageReceived)
            {
                Debug.Log("talkigng = " + talking);
                animator.SetTalking(talking);
                isMessageReceived = false;
            }
        }
    }
}