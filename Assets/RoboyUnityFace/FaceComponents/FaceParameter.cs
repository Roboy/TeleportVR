using UnityEngine;
using System.Collections;
using System;

public class FaceParameter : MonoBehaviour {

	public Vector3 min = Vector3.zero;
    public Vector3 max = Vector3.one;
    public Vector3 value = Vector3.one/2.0f;
	//public Animator
	Vector3 Value{
		get{
			return this.min+(Vector3.Scale(this.value,this.max-this.min));
		}

	}

    public void SetValueX(float value)
    {
        this.value = new Vector3(Mathf.Clamp01(value), this.value.y, this.value.z);
    }
    public void SetValueY(float value)
    {
        this.value = new Vector3( this.value.x, Mathf.Clamp01(value), this.value.z);
    }
    public void SetValueZ(float value)
    {
        this.value = new Vector3(this.value.x, this.value.y, Mathf.Clamp01(value));
    }

    public enum ParamType{ PositionX,PositionY }
	// Use this for initialization
	void Start () {
	}
    	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = this.Value;
	}

}
