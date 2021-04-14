using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOffset : MonoBehaviour
{

    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position;
        Debug.Log(transform.rotation);
    }
}
