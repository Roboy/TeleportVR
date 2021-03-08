using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecalibrateUpperBody : Singleton<RecalibrateUpperBody>
{
    public GameObject HeadPos;

    public void Calibrate() {
        //transform.position = HeadPos.transform.position - Vector3.up * 0.5f;
    }
}
