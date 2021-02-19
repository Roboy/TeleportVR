using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class EmotionSubscriber : UnitySubscriber<MessageTypes.RoboyControl.Emotion>
    {
        private RoboyAnimator animator;
        private bool isMessageReceived;
        private string emotion;

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

        protected override void ReceiveMessage(MessageTypes.RoboyControl.Emotion message)
        {
            emotion = message.emotion;
            isMessageReceived = true;
        }

        private void ProcessMessage()
        {
            if (isMessageReceived)
            {
                animator.SetEmotion(emotion);
                isMessageReceived = false;
            }
        }
    }
}