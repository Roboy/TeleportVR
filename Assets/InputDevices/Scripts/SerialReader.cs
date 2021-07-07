using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO.Ports;

namespace RudderPedals
{
    public class SerialReader
    {
        private SerialPort stream;
        private string port;
        private int baudRate;
        private float readTimeout, refresh;

        /// <summary>
        /// Serial Reader reads data from a given serial port.
        /// </summary>
        /// <param name="port">Serial port to connect to</param>
        /// <param name="baudRate">Serial baud rate</param>
        /// <param name="readTimeout">Timeout for a request (seconds)</param>
        /// <param name="refresh">Serial polling interval (seconds)</param>
        public SerialReader(string port = "COM6", int baudRate = 9600, float readTimeout = 0.01f, float refresh = 0.1f)
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
                Debug.Log($"Opened serial connection on {port} @ {baudRate}");

            }
            catch (System.IO.IOException)
            {
                Debug.LogError($"Error while opening serial connection on {port}@{baudRate}");
            }
        }

        /// <summary>
        /// Finalizer performing cleanup (closing open ressources)
        /// </summary>
        ~SerialReader()
        {
            this.stream.Close();
        }

        public IEnumerator readAsyncContinously(Action<string> callback, Action<string> onError = null)
        {
            string data = null;
            string res;
            while (true)
            {
                // request data
                stream.WriteLine("GET");
                stream.BaseStream.Flush();
                res = null;
                try
                {
                    res = stream.ReadLine();
                }
                catch (TimeoutException)
                {
                    Debug.LogError($"Timeout reading from serial {port} @ {baudRate}");
                    continue;
                }
                
                if (res.StartsWith("ERROR:"))
                {
                    if (onError != null)
                    {
                        onError(res);
                    }
                    yield break;
                }

                // only publish if data is new
                if (!res.Equals(data))
                {
                    data = res;
                    callback(data);
                }
                yield return new WaitForSecondsRealtime(refresh);
            }
        }

    }

}
