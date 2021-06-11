using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHUD : Singleton<PauseHUD>
{

    [Range(0, 1)]
    public float show = 0;

    public GameObject paused;
    private Vector3 start, end;
    private Vector3 localScale;
    
    // Start is called before the first frame update
    void Start()
    {
        start = transform.localPosition+ new Vector3(0, 0.5f, 0);
        end = transform.localPosition;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition= Vector3.Lerp(start, end, show);
        if (show > 0)
        {
            paused.SetActive(true);
            transform.localScale = localScale;
        } 
        else
        {
            paused.SetActive(false);
            transform.localScale = Vector3.zero;
        }
    }
}
