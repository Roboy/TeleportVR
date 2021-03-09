using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            /*if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }*/

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    T[] instances;

                    // Search for existing instance.
                    instances = (T[])FindObjectsOfType(typeof(T));

                    if (instances.Length == 0)
                        Debug.LogError("No instance of " + typeof(T) + " found in scene");
                    else if (instances.Length > 1)
                        Debug.LogError("More than one instance of " + typeof(T) + " found in scene");
                    else
                        m_Instance = instances[0];
                }

                return m_Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}