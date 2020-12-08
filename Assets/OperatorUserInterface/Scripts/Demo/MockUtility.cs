using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;

public class MockUtility : MonoBehaviour
{
    internal RosPublisher rosPublisher;

    // Start is called before the first frame update
    void Start()
    {
        rosPublisher = GameObject.FindGameObjectWithTag("RosManager").GetComponent<RosPublisher>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
