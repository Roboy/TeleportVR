using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{

    [Range(0, 1)]
    public float show = 0;
    public GameObject child;

    [Header("UI Buttons")]
    public TouchButton switchScene;

    private Vector3 startPos, endPos;
    private Vector3 localScale;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition + new Vector3(0, 0.5f, 0);
        endPos = transform.localPosition;
        localScale = transform.localScale;
        // buttons init
        switchScene.OnTouchEnter(() => Debug.Log("Changing scene to HUD"));
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Slerp(startPos, endPos, show);
        if (show > 0)
        {
            child.SetActive(true);
            transform.localScale = localScale;
        }
        else
        {
            child.SetActive(false);
            transform.localScale = Vector3.zero;
        }
    }

}
