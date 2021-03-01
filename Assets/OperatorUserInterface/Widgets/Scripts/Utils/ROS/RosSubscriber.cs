using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
//using RosSharp.RosBridgeClient.MessageTypes.Roboy;

namespace Widgets
{
    public class RosSubscriber : UnitySubscriber<RosSharp.RosBridgeClient.MessageTypes.Std.String>
    {
        /// <summary>
        /// Holds a queue of messages to be read one after the other from the manager.
        /// </summary>
        private Queue<RosJsonMessage> interfaceMessageQueue;
        /// <summary>
        /// Start this instance.
        /// </summary>
        protected override void Start()
        {
            interfaceMessageQueue = new Queue<RosJsonMessage>();
            //StartCoroutine(startSubscriber(1.0f));
        }

        /// <summary>
        /// Enqueues the interface message queue.
        /// </summary>
        /// <param name="msg">The interface Message.</param>
        public void EnqueueInterfaceMessage(RosJsonMessage msg)
        {
            interfaceMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Dequeues the interface message queue.
        /// </summary>
        /// <returns>The interface message.</returns>
        public RosJsonMessage DequeueInterfaceMessage()
        {
            return interfaceMessageQueue.Dequeue();
        }

        /// <summary>
        /// Counts the number of objects in the queue.
        /// </summary>
        /// <returns>The queue count.</returns>
        public int MessageQueueCount()
        {
            return interfaceMessageQueue.Count;
        }

        public bool IsEmpty()
        {
            return interfaceMessageQueue.Count == 0;
        }

        /// <summary>
        /// Starts the subscriber.
        /// </summary>
        /// <returns>The subscriber.</returns>
        /// <param name="waitTime">Wait time.</param>
        private IEnumerator startSubscriber(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                base.Start();
                break;
            }
        }

        /// <summary>
        /// Receives the message by enqueueing the queue.
        /// </summary>
        /// <param name="message">Message.</param>
        protected override void ReceiveMessage(RosSharp.RosBridgeClient.MessageTypes.Std.String message)
        {
            Debug.Log("Received Message");
            Debug.Log(message.data);
            RosJsonMessage msg = JsonUtility.FromJson<RosJsonMessage>(message.data);
            EnqueueInterfaceMessage(msg);
        }
    }
}
