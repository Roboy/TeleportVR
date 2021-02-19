using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class EyePositionSubscriber : UnitySubscriber<MessageTypes.Geometry.PoseStamped>
    {
        public GameObject eyeLeft;
        public GameObject eyeRight;
        private bool isMessageReceived;

        private GameObject targetEye;
        private Vector3 newPos;
        private Vector3 delta;
        private string frame;
        private const float eyeWidth = 214.0f;
        private const float eyeHeight = 177.0f;
        private bool changeBoth = false;
        private Vector3 initPosLeft;
        private Vector3 initPosRight;

        protected override void Start()
        {
            base.Start();
            initPosLeft = eyeLeft.transform.position;
            initPosRight = eyeRight.transform.position;
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.PoseStamped message)
        {
            delta = GetPosition(message);
            frame = GetFrame(message);
            // Debug.Log("message arrived");

            //targetEye.transform.SetPositionAndRotation(newPos, targetEye.transform.rotation);

            isMessageReceived = true;
            //Debug.Log("received true");
        }

        private void ProcessMessage()
        {
            switch (frame)
            {
                case "eye_left":
                    targetEye = eyeLeft;

                    break;
                case "eye_right":
                    targetEye = eyeLeft;
                    break;
                default:
                    //Debug.LogWarning("unknown eye frame");
                    changeBoth = true;
                    var offset = new Vector3(delta.x * eyeWidth / 4.0f, delta.y * eyeHeight / 4.0f, 0);
                    //var offset = new Vector3(-30 + delta.x * eyeWidth - eyeWidth / 2.0f, 30 -delta.y * eyeHeight + eyeHeight / 2.0f, 0);
                    //var offset = new Vector3(0, 30 -delta.y * eyeHeight + eyeHeight / 2.0f, 0);
                    eyeRight.transform.SetPositionAndRotation(initPosRight + offset, eyeRight.transform.rotation);
                    eyeLeft.transform.SetPositionAndRotation(initPosLeft + offset, eyeLeft.transform.rotation);
                    return;
            }
            Debug.Log("selected target eye: " + targetEye.name);
            newPos = targetEye.transform.position + delta;
            Debug.Log(newPos);
            targetEye.transform.SetPositionAndRotation(newPos, targetEye.transform.rotation);
            //PublishedTransform.position = position;
            //PublishedTransform.rotation = rotation;
            isMessageReceived = false;
            changeBoth = false;
        }

        private Vector3 GetPosition(MessageTypes.Geometry.PoseStamped message)
        {
            return new Vector3(
                (float)message.pose.position.x,
                (float)message.pose.position.y,
                (float)message.pose.position.z);
        }

        private Quaternion GetRotation(MessageTypes.Geometry.PoseStamped message)
        {
            return new Quaternion(
                (float)message.pose.orientation.x,
                (float)message.pose.orientation.y,
                (float)message.pose.orientation.z,
                (float)message.pose.orientation.w);
        }

        private string GetFrame(MessageTypes.Geometry.PoseStamped message)
        {
            return message.header.frame_id;
        }
    }
}