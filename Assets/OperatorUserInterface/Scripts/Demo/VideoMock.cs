using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;

public class VideoMock : MockUtility
{
    int round;
    int temperature = 4;
    float tempTimer;
    
#if ROSSHARP
    void Update()
    {
        // update the temperature every second
        tempTimer -= Time.deltaTime;
        if (tempTimer <= 0)
        {
            // update the text 
            RosJsonMessage textMessage = RosJsonMessage.CreateTextMessage(37, "Temperature:\n" + temperature + " C", 0, null);
            rosJsonPublisher.PublishMessage(textMessage);

            // update the graph
            RosJsonMessage demoMessage = RosJsonMessage.CreateGraphMessage(1, temperature, 0, new byte[] { 10, 10, 250 });
            rosJsonPublisher.PublishMessage(demoMessage);
            tempTimer = 1;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            round++;
            if (round == 1)
            {
                RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, "Toxic gas detected!", 2,
                    new byte[] { 255, 10, 0, 255 });
                rosJsonPublisher.PublishMessage(toastrMessage);
            }
            else if (round == 2)
            {
                RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, "Smell of Zombies detected!", 2,
                    new byte[] { 255, 10, 0, 255 });
                rosJsonPublisher.PublishMessage(toastrMessage);
            }
            else if (round == 3 || round == 4 || round == 5 || round == 7 || round == 8 || round == 10 || round == 11)
            {
                temperature -= 1;
            }
            else if (round == 6)
            {
                // activate temperature graph (Temp is 1)
                GameObject.FindGameObjectWithTag("WidgetsChild").transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (round == 9)
            {
                GameObject.FindGameObjectWithTag("WidgetsChild").transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
#endif
}
