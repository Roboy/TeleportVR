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


        public SerialReader(string port = "COM6", int baudRate = 9600, float readTimeout = 0.01f, float refresh = 0.1f)
        {
            this.port = port;
            this.baudRate = baudRate;
            this.readTimeout = 0;
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
                    res = null;
                }
                if (res != null)
                {
                    if (res.StartsWith("ERROR:"))
                    {
                        if (onError != null)
                        {
                            onError(res);
                        }
                        yield break;
                    }
                    // only publish if data is new
                    else if (!res.Equals(data))
                    {
                        data = res;
                        callback(data);
                    }
                }

                yield return new WaitForSecondsRealtime(refresh);
            }
        }

    }

}
