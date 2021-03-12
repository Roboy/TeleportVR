using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;
using ContactPoint = RosSharp.RosBridgeClient.MessageTypes.RoboySimulation.ContactPoint;
using Collision = RosSharp.RosBridgeClient.MessageTypes.RoboySimulation.Collision;
using Vector3 = RosSharp.RosBridgeClient.MessageTypes.Geometry.Vector3;

public class CollisionPublisher : RosPublisher<Collision>
{
    private int collisionMessageSize = 9;
    
    public void PublishCollision(float[] rawData)
    {
        if (rawData.Length % collisionMessageSize != 0)
        {
            Debug.LogWarning("Invalid collision array size: " + rawData.Length + " for collision msg size " +
                             collisionMessageSize);
        }

        int collisionCount = rawData.Length / collisionMessageSize;
        Header header = new Header();
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
            
            //PublishMessage(point);
        }

        Collision collision = new Collision(header, contactPoints);
        PublishMessage(collision);
        print("published: " + collision);
    }

    private void Update()
    {
        // Mock
        if (Input.GetKeyDown(KeyCode.X))
        {
            /*if (contactPointPublicationId == "")
            {
                contactPointPublicationId = rosConnector.RosSocket.Advertise<>(Topic);
            }*/
            PublishCollision(new []{0f, 1, 2, 3, 4, 5, 6, 7, 8});
        }
    }
}
