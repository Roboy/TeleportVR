using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchButton : MonoBehaviour
{
    public string text
    {
        get { return _text; }
        set
        {
            _text = value;
            textMeshPro.SetText(_text);
        }
    }

    public int fontSize
    {
        get { return _fontSize; }
        set
        {
            _fontSize = value;
            textMeshPro.fontSize = _fontSize;
        }
    }
    public Color textColor
    {
        get { return _textColor; }
        set
        {
            _textColor = value;
            textMeshPro.color = _textColor;
        }
    }

    // do not use directly, only for internal storage.
    // use above getter / setter for interacting.
    private string _text;
    private int _fontSize;
    private Color _textColor;

    private TextMeshProUGUI textMeshPro;
    private TouchButtonActivationVolume activationVolume;

    // Start is called before the first frame update
    void Start()
    {
        // Find relevant children
        textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        activationVolume = gameObject.GetComponentInChildren<TouchButtonActivationVolume>();
    }

    public void OnTouchEnter(System.Action callback) 
    {
        activationVolume.enterCallbacks.Add(callback);
    }

    public void OnTouchExit(System.Action callback)
    {
        activationVolume.exitCallbacks.Add(callback);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
