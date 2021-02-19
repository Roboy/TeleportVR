using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardMover : MonoBehaviour {
    public KeyCode keyUp;
    public KeyCode keyDown;
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode keyZoomIn;
    public KeyCode keyZoomOut;
    public Vector3 scaleStep;

    private float delta = 0.1f;
    public Vector3 startOffset;
    public Vector3 startScale;
    public string saveName;

    static Vector3 ParseVector3(string sourceString)
    {

        string outString;
        Vector3 outVector3 = new Vector3();
        string[] splitString;

        // Trim extranious parenthesis

        outString = sourceString.Substring(1, sourceString.Length - 2);

        // Split delimted values into an array

        splitString = outString.Split(","[0]);

        // Build new Vector3 from array elements

        outVector3.x = float.Parse(splitString[0]);
        outVector3.y = float.Parse(splitString[1]);
        outVector3.z = float.Parse(splitString[2]);

        return outVector3;
    }

    // Use this for initialization
    void Start () {
        PlayerPrefs.DeleteAll();
        transform.position = startOffset;
        transform.localScale = startScale;
        try
        {
            Debug.Log("Start:"+PlayerPrefs.GetString(transform.name + "_start"));
            Debug.Log("Scale:"+PlayerPrefs.GetString(transform.name + "_scale"));
            Vector3 spos = ParseVector3(PlayerPrefs.GetString(transform.name + "_start"));
            Vector3 sscale = ParseVector3(PlayerPrefs.GetString(transform.name + "_scale"));
            transform.position = spos;
            transform.localScale = sscale;
            Debug.Log("loaded:"+ transform.name + " startOffset" + spos + " localScale:" + sscale);
        }
        finally {
        }
    }

    // Update is called once per frame
    void Update ()
    {
        bool changed = false;
        if (Input.GetKeyDown(keyUp))
        {
            transform.position += new Vector3(0, delta, 0);
            Debug.Log(transform.name+" startOffset" + transform.position + " localScale:" + transform.localScale);
            changed = true;
        }
        if (Input.GetKeyDown(keyDown))
        {
            transform.position += new Vector3(0, -delta, 0);
            Debug.Log(transform.name + " startOffset" + transform.position + " localScale:" + transform.localScale);
            changed = true;
        }
        if (Input.GetKeyDown(keyLeft))
        {
            transform.position += new Vector3(delta, 0, 0);
            Debug.Log(transform.name + " startOffset" + transform.position + " localScale:" + transform.localScale);
            changed = true;
        }
        if (Input.GetKeyDown(keyRight))
        {
            transform.position += new Vector3(-delta, 0, 0);
            Debug.Log(transform.name + " startOffset" + transform.position + " localScale:" + transform.localScale);
            changed = true;
        }
        if (Input.GetKeyDown(keyZoomIn))
        {
            transform.localScale += scaleStep;
            Debug.Log(transform.name + " startOffset" + transform.position + " localScale:" + transform.localScale);
            changed = true;
        }
        if (Input.GetKeyDown(keyZoomOut))
        {
            transform.localScale -= scaleStep;
            Debug.Log(transform.name + " startOffset" + transform.position + " localScale:" + transform.localScale);
            changed = true;
        }
        if (changed)
        {
            PlayerPrefs.SetString(transform.name + "_start", transform.position.ToString());
            PlayerPrefs.SetString(transform.name + "_scale", transform.localScale.ToString());
            PlayerPrefs.Save();
            Debug.Log("saved!");
        }


    }
}
