using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Messages.Roboy;
/// <summary>
/// Motor status subscriber.
/// </summary>
public class MotorStatusSubscriber : Subscriber<RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus>
{
    /// <summary>
    /// Holds a queue of messages to be read one after the other from the manager.
    /// </summary>
    private Queue<RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus> motorStatusQueue;
    /// <summary>
    /// Start this instance.
    /// </summary>
    protected override void Start()
    {
        motorStatusQueue = new Queue<RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus>();
        StartCoroutine(startSubscriber(1.0f));
    }
    /// <summary>
    /// Enqueues the motor message queue.
    /// </summary>
    /// <param name="msg">Message.</param>
    public void EnqueueMotorMessage(RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus msg)
    {
        motorStatusQueue.Enqueue(msg);
    }
    /// <summary>
    /// Dequeues the motor message queue.
    /// </summary>
    /// <returns>The motor message.</returns>
    public RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus DequeueMotorMessage()
    {
        return motorStatusQueue.Dequeue();
    }
    /// <summary>
    /// Counts the number of objects in the queue.
    /// </summary>
    /// <returns>The queue count.</returns>
    public int MessageQueueCount()
    {
        return motorStatusQueue.Count;
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
    protected override void ReceiveMessage(RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus message)
    {
        Debug.Log(message.angle);
        EnqueueMotorMessage(message);      
    }
}
