#if ROSSHARP
using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;

public class CloseCagePublisher : RosPublisher<Empty>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //InitExoforceResponse response = new InitExoforceResponse();
            //_cageRosConnector.RosSocket.Publish(initResponsePublicationId, response);
            //print("Send a response");

            //if (CageInterface.cageIsConnected)
            //{
                Publish();
            //}
        }
    }

    public void Publish()
    {
        // Send disconnect message
        PublishMessage(new Empty());
    }
}
#endif
