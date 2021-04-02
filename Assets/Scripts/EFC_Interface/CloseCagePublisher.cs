#if ROSSHARP
using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;

public class CloseCagePublisher : RosPublisher<Empty>
{
    // Allows to send a closeCage message with the keyboard
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Publish();
        }
    }

    // publish a message to close the cage to the EFC Team
    public void Publish()
    {
        // Send disconnect message
        PublishMessage(new Empty());
    }
}
#endif
