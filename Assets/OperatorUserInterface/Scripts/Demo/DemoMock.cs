using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;

public class DemoMock : MockUtility
{
#if ROSSHARP
    bool sensgloves_working = true;

    // Update is called once per frame
    void Update()
    {
        // Sensglove failure
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("S is pressed");
            if (sensgloves_working)
            {
                RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, "Senseglove disconnected", 5,
                    new byte[] { 255, 40, 15, 255 });
                rosJsonPublisher.PublishMessage(toastrMessage);
                //RosJsonMessage iconMessage = RosJsonMessage.CreateIconMessage(20, "Gloves-OFF_01a");
                RosJsonMessage iconMessage = RosJsonMessage.CreateIconMessage(20, "GlovesRed");
                // GlovesYellow GlovesRed
                rosJsonPublisher.PublishMessage(iconMessage);
                RosJsonMessage textMessage = RosJsonMessage.CreateTextMessage(30, "Not connected\nThe cable disconnected", 0, null);
                rosJsonPublisher.PublishMessage(textMessage);
            }
            else
            {
                RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, "Senseglove reconnected", 5, 
                    new byte[] { 100, 250, 10 , 255});
                rosJsonPublisher.PublishMessage(toastrMessage);
                //RosJsonMessage iconMessage = RosJsonMessage.CreateIconMessage(20, "Gloves-ON_01a");
                RosJsonMessage iconMessage = RosJsonMessage.CreateIconMessage(20, "GlovesGreen");
                rosJsonPublisher.PublishMessage(iconMessage);
                RosJsonMessage textMessage = RosJsonMessage.CreateTextMessage(30, "Connected\nSensegloves fully functioning", 0, null);
                rosJsonPublisher.PublishMessage(textMessage);
            }
            sensgloves_working = !sensgloves_working;
        }
    }
#endif
}
