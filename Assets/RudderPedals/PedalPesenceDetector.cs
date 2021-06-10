using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO.Ports;

namespace RudderPedals
{
    public class PedalPesenceDetector
    {
        private SerialPort stream;
        private string port;
        private int baudRate;
        private float readTimeout, refresh;


        public PedalPesenceDetector(string port = "COM6", int baudRate = 9600, float readTimeout = 0.01f, float refresh = 0.1f)
        {
            this.port = port;
            this.baudRate = baudRate;
            this.readTimeout = readTimeout;
            this.refresh = refresh;

            this.stream = new SerialPort(port, baudRate);
            this.stream.ReadTimeout = (int)(1000 * readTimeout);
            this.stream.DtrEnable = true;
            this.stream.RtsEnable = true;
            try
            {
                stream.Open();
                Debug.Log($"Opened serial connection on {port}@{baudRate}");

            }
            catch (System.IO.IOException)
            {
                Debug.LogError($"Error while opening serial connection on {port}@{baudRate}");
            }
        }

        public IEnumerator readAsyncContinously(Action<bool, bool> callback)
        {
            //DateTime initTime = DateTime.Now;
            //DateTime nowTime;
            //TimeSpan diff = default(TimeSpan);
            string data = null;
            string tmp = null;
            while (true)
            {
                // request data
                stream.WriteLine("GET");
                stream.BaseStream.Flush();
                tmp = null;
                try
                {
                    tmp = stream.ReadLine();
                }
                catch (TimeoutException)
                {
                    tmp = null;
                }
                if (tmp.StartsWith("ERROR:"))
                {
                    Debug.LogError(tmp);
                    yield break;
                }

                // only publish if data is new
                if (tmp != null && !tmp.Equals(data))
                {
                    data = tmp;
                    bool[] parsed = parseSerial(data);
                    if (parsed != null)
                    {
                        callback(parsed[0], parsed[1]);
                    }
                }
                yield return new WaitForSeconds(refresh);
            }
        }

        private bool[] parseSerial(string data)
        {
            try
            {
                if (data == null) return null;

                string[] args = data.Split(',');
                int left = int.Parse(args[0]);
                int right = int.Parse(args[1]);
                return new bool[] { left != 0, right != 0 };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
