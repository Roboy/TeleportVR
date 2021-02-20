using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

	public int frequency;

	public bool lookForward;
	public bool reset;

	public Transform[] items;

    private List<Transform> instances;

    private void Awake() {
        if (instances == null)
        {
            instances = new List<Transform>();
        }
        instances.Clear();
        Transform[] t = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i].name.Contains("(Clone)")) {
                if (!Application.isPlaying)
                    DestroyImmediate(t[i].gameObject);
                else
                    Destroy(t[i].gameObject);
            }
        }
		if (frequency <= 0 || items == null || items.Length == 0) {
			return;
		}
		float stepSize = frequency * items.Length;
		if (spline.Loop || stepSize == 1) {
			stepSize = 1f / stepSize;
		}
		else {
			stepSize = 1f / (stepSize - 1);
		}
        for (int p = 0, f = 0; f < frequency; f++) {
			for (int i = 0; i < items.Length; i++, p++) {
				Transform item = Instantiate(items[i]) as Transform;
                instances.Add(item);
                Vector3 position = spline.GetPoint(p * stepSize);
				item.transform.localPosition = position;
				if (lookForward) {
					item.transform.LookAt(position + spline.GetDirection(p * stepSize));
				}
				item.transform.parent = transform;
                // scale the mouth size with the canvas
                item.transform.localScale = 50 * Vector3.one;
			}
		}
	}

    private void Update()
    {
			if(reset){
				Awake();
				reset = false;
			}
        if (frequency <= 0 || items == null || items.Length == 0)
        {
            return;
        }
				//if(items.Length <= 1) Awake();
        float stepSize = frequency * items.Length;
        if (spline.Loop || stepSize == 1)
        {
            stepSize = 1f / stepSize;
        }
        else
        {
            stepSize = 1f / (stepSize - 1);
        }
        for (int p = 0;p < instances.Count; p++)
        {
            Transform item = instances[p];
            Vector3 position = spline.GetPoint(p * stepSize);
            item.transform.position = position;
            if (lookForward)
            {
                item.transform.LookAt(position + spline.GetDirection(p * stepSize));
            }
            //item.transform.parent = transform;
        }
    }

}
