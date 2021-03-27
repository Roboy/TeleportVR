using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RosSharp.RosBridgeClient;
/// <summary>
/// Mock motor status publisher.
/// </summary>
/*public class MockMotorStatusPublisher : Publisher<RosSharp.RosBridgeClient.MessageTypes.Std.Messages.Roboy.MotorStatus>
{
    /// <summary>
    ///  Start method of MockMotorStatusPublisher.
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
            break;
        }
    }
    /// <summary>
    /// Publish a mock motor message.
    /// </summary>
    public void PublishMotorMessage()
    {
        // TEST MESSAGE FOR MOTOR STATUS      
        RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus message = new RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus();
        message.id = 0;
        message.power_sense = true;
        message.pwm_ref = new int[] { 10 };
        message.position = new int[] { 2 };
        message.velocity = new int[] { 20 };
        message.displacement = new int[] { 0 };
        message.current = new short[] { 43 };
        message.angle = new int[] { 20 };

        // TEST MESSAGE FOR MOTOR STATUS      
        RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus message_0 = new RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus();
        message_0.id = 1;
        message_0.power_sense = true;
        message_0.pwm_ref = new int[] { 10 };
        message_0.position = new int[] { 2 };
        message_0.velocity = new int[] { 20 };
        message_0.displacement = new int[] { 0 };
        message_0.current = new short[] { 2 };
        message_0.angle = new int[] { 20 };

        PublishMessage(message);
        PublishMessage(message_0);
    }
    /// <summary>
    /// Publishs the mock motor status message.
    /// </summary>
    /// <param name="message">message</param>
    private void PublishMessage(RosSharp.RosBridgeClient.Messages.Roboy.MotorStatus message)
    {
        Publish(message);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Publishing...");
            PublishMotorMessage();
        }
    }
}*/
