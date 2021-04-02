using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCamera : MonoBehaviour
{
    public GameObject[] Cams;
    int currentCam;

    public Text txtCameraInfo;

    void Start()
    {
        currentCam = 0;
        Cams[currentCam].GetComponent<Camera>().enabled = true;
        txtCameraInfo.text = "View: " + Cams[0].name;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cams[currentCam].GetComponent<Camera>().enabled = false;
            currentCam = (++currentCam) % Cams.Length;
            Cams[currentCam].GetComponent<Camera>().enabled = true;
            txtCameraInfo.text = "View: " + Cams[currentCam].name;
        }
    }
}
