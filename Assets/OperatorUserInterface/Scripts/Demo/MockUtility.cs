﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;

public class MockUtility : MonoBehaviour
{
    internal RosJsonPublisher rosJsonPublisher;

    // Start is called before the first frame update
    void Start()
    {
        rosJsonPublisher = GameObject.FindGameObjectWithTag("RosManager").GetComponent<RosJsonPublisher>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
