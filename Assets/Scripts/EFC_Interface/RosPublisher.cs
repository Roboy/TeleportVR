using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;
using Collision = RosSharp.RosBridgeClient.MessageTypes.RoboySimulation.Collision;

public class RosPublisher<T> : UnityPublisher<T> where T : Message
{
    protected bool started = false;
    
    /// <summary>
    ///  Start method of InterfaceMessage.
    /// Starts a coroutine to initialize the publisher after 1 second to prevent race conditions.
    /// </summary>
    protected override void Start()
    {
        StartCoroutine(StartPublisher(1.0f));
    }
    /// <summary>
    /// Starts the publisher.
    /// </summary>
    /// <returns>The publisher.</returns>
    /// <param name="waitTime">Wait time.</param>
    private IEnumerator StartPublisher(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            base.Start();
            started = true;
            break;
        }
    }
    
    /// <summary>
    /// Publishs the json string as a ros message.
    /// </summary>
    /// <param name="message">message</param>
    protected void PublishMessage(T message)
    {
        if (started)
        {
            Publish(message);
            print("Published a new message of type " + typeof(T));
        }
        else
        {
            Debug.LogWarning("Publisher for " + typeof(T) + " has not been started yet!");
        }
    }
}
