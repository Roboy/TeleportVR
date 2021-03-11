using System.Collections;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.IO;
using System;

namespace Widgets
{
    /// <summary>
    /// The RosPublisher class is mainly for mocking and debugging purposes.
    /// </summary>
    public class RosJsonPublisher : RosPublisher<RosSharp.RosBridgeClient.MessageTypes.Std.String>
    {
        private float temperature = 20;

        /// <summary>
        /// Publishs the json string as a ros message.
        /// </summary>
        /// <param name="message">message</param>
        private void PublishMessage(RosSharp.RosBridgeClient.MessageTypes.Std.String message)
        {
            if (started)
            {
                Publish(message);
            }
        }

        /// <summary>
        /// Publish a RosJsonMessage on ros for mocking
        /// </summary>
        /// <param name="demoMessage"></param>
        public void PublishMessage(RosJsonMessage demoMessage)
        {
            string jsonString = JsonUtility.ToJson(demoMessage);
            RosSharp.RosBridgeClient.MessageTypes.Std.String tmpMessage = new RosSharp.RosBridgeClient.MessageTypes.Std.String(jsonString);
            PublishMessage(tmpMessage);

            WriteMessageToFile("demoMessage", jsonString);
        }

        /// <summary>
        /// Publish a graph demo message with a random temperature
        /// </summary>
        private void PublishGraphDemoMessage()
        {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            double cur_time = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            byte[] col = new byte[] { 255, 255, 255, 255 };
            if (temperature > 30)
            {
                col = new byte[] { 255, 20, 5, 255 };
            }
            else if (temperature < 20)
            {
                col = new byte[] { 5, 10, 255, 255 };
            }
            RosJsonMessage demoMessage = RosJsonMessage.CreateGraphMessage(1, temperature, cur_time, col);
            PublishMessage(demoMessage);
        }

        /// <summary>
        /// Writes the json string into a file for easier debugging
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="json">json string to be written into file</param>
        /// <returns></returns>
        private bool WriteMessageToFile(string fileName, string json)
        {
            string filePath = Application.persistentDataPath + fileName + ".json";

            print("Saved demoMessage at " + filePath);

            File.WriteAllText(filePath, json);

            return true;
        }

        bool toggle = false;

        /// <summary>
        /// Sends mock messages to toggle the senseglove icon.
        /// </summary>
        private void PublishIconDemoMessage()
        {
            RosJsonMessage demoMessage;
            if (toggle)
            {
                demoMessage = RosJsonMessage.CreateIconMessage(20, "SenseGlove_0");

            }
            else
            {
                demoMessage = RosJsonMessage.CreateIconMessage(20, "SenseGlove_1");
            }
            PublishMessage(demoMessage);

            toggle = !toggle;
        }

        /// <summary>
        /// Sends mock messages for a toastr.
        /// </summary>
        private void PublishToastrDemoMessage()
        {
            RosJsonMessage demoMessage = RosJsonMessage.CreateToastrMessage(10, "Hello Roboy", 2, null);
            PublishMessage(demoMessage);
        }

        /// <summary>
        /// For keyboard input to trigger mock messages.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                temperature -= 2;
                PublishGraphDemoMessage();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                temperature += 2;
                PublishGraphDemoMessage();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                PublishIconDemoMessage();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                PublishToastrDemoMessage();
            }
        }
    }
}