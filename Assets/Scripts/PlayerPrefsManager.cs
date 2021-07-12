using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlayerPrefsManager : MonoBehaviour
{

    public bool clearPlayerPrefs = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (clearPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Cleared PlayerPrefs");
            clearPlayerPrefs = false;
        }
    }
}
