#if ROSSHARP
using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;
using ContactPoint = RosSharp.RosBridgeClient.MessageTypes.RoboySimulation.ContactPoint;
using Collision = RosSharp.RosBridgeClient.MessageTypes.RoboySimulation.Collision;
using Vector3 = RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3;

public class CollisionPublisher : RosPublisher<Collision>
{
    private int collisionMessageSize = 9;
    
    /// <summary>
    /// Publish a collision received from animus to the EFC Team via Animus.
    /// </summary>
    /// <param name="rawData">The data that should be forwarded.</param>
    public void PublishCollision(float[] rawData)
    {
        if (rawData.Length % collisionMessageSize != 0)
        {
            Debug.LogWarning("Invalid collision array size: " + rawData.Length + " for collision msg size " +
                             collisionMessageSize);
        }

        int collisionCount = rawData.Length / collisionMessageSize;
        ContactPoint[] contactPoints = new ContactPoint[collisionCount];
        for (int i = 0; i < collisionCount; i++)
        {
            // the current offset
            int o = i * collisionMessageSize;
            ContactPoint point = new ContactPoint(
                (long)(rawData[o]), 
                new Vector3(rawData[o+1], rawData[o+2], rawData[o+3]),
                new Vector3(rawData[o+4], rawData[o+5], rawData[o+6]),
                (double)(rawData[7]),
                (double)(rawData[8])
                );
            contactPoints[i] = point;
        }

        Collision collision = new Collision(contactPoints);
        PublishMessage(collision);
    }

    private void Update()
    {
        // // Allows to send a mock collision message with the keyboard
        if (Input.GetKeyDown(KeyCode.X))
        {
            PublishCollision(new []{0f, 1, 2, 3, 4, 5, 6, 7, 8});
        }
    }
}
#endif
