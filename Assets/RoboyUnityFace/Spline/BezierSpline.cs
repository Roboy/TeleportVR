using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour {

	[SerializeField]
	private Vector3[] _points;

    protected virtual Vector3 getpoint(int id)
    {
        return _points[id];
    }

    protected virtual void setpoint(int id,Vector3 val)
    {
        _points[id] = val;
    }

    protected virtual int numpoints { get { return numpoints; } }

    private void addtopoint(int id, Vector3 val)
    {
        setpoint(id,getpoint(id)+val);
    }
	[SerializeField]
	private BezierControlPointMode[] modes;

	[SerializeField]
	private bool loop;

	public bool Loop {
		get {
			return loop;
		}
		set {
			loop = value;
			if (value == true) {
				modes[modes.Length - 1] = modes[0];
				SetControlPoint(0, getpoint(0));
			}
		}
	}

	public int ControlPointCount {
		get {
			return numpoints;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return getpoint(index);
	}

	public void SetControlPoint (int index, Vector3 point) {
		if (index % 3 == 0) {
			Vector3 delta = point - getpoint(index);
			if (loop) {
				if (index == 0) {
					addtopoint(1,delta);
                    addtopoint(numpoints - 2,delta);
                    addtopoint(numpoints - 1,point);
				}
				else if (index == numpoints - 1) {
                    setpoint(0, point);
                    addtopoint(1, delta);
                    addtopoint(index - 1, delta);
                }
				else {
                    addtopoint(index - 1, delta);
                    addtopoint(index + 1, delta);
                }
            }
			else {
				if (index > 0)
                {
                    addtopoint(index - 1, delta);
                }
                if (index + 1 < numpoints) {
                    addtopoint(index + 1, delta);
                }
			}
		}
		setpoint(index,point);
		EnforceMode(index);
	}

	public BezierControlPointMode GetControlPointMode (int index) {
		return modes[(index + 1) / 3];
	}

	public void SetControlPointMode (int index, BezierControlPointMode mode) {
		int modeIndex = (index + 1) / 3;
		modes[modeIndex] = mode;
		if (loop) {
			if (modeIndex == 0) {
				modes[modes.Length - 1] = mode;
			}
			else if (modeIndex == modes.Length - 1) {
				modes[0] = mode;
			}
		}
		EnforceMode(index);
	}

	private void EnforceMode (int index) {
		int modeIndex = (index + 1) / 3;
		BezierControlPointMode mode = modes[modeIndex];
		if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
			return;
		}

		int middleIndex = modeIndex * 3;
		int fixedIndex, enforcedIndex;
		if (index <= middleIndex) {
			fixedIndex = middleIndex - 1;
			if (fixedIndex < 0) {
				fixedIndex = numpoints - 2;
			}
			enforcedIndex = middleIndex + 1;
			if (enforcedIndex >= numpoints) {
				enforcedIndex = 1;
			}
		}
		else {
			fixedIndex = middleIndex + 1;
			if (fixedIndex >= numpoints) {
				fixedIndex = 1;
			}
			enforcedIndex = middleIndex - 1;
			if (enforcedIndex < 0) {
				enforcedIndex = numpoints - 2;
			}
		}

		Vector3 middle = getpoint(middleIndex);
		Vector3 enforcedTangent = middle - getpoint(fixedIndex);
		if (mode == BezierControlPointMode.Aligned) {
			enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, getpoint(enforcedIndex));
		}
		setpoint(enforcedIndex,middle + enforcedTangent);
	}

	public int CurveCount {
		get {
			return (numpoints - 1) / 3;
		}
	}

	public Vector3 GetPoint (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = numpoints - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetPoint(getpoint(i), getpoint(i+1), getpoint(i+2), getpoint(i+3), t));
	}
	
	public Vector3 GetVelocity (float t) {
		int i;
		if (t >= 1f) {
			t = 1f;
			i = numpoints - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint(Bezier.GetFirstDerivative(getpoint(i), getpoint(i + 1), getpoint(i + 2), getpoint(i + 3), t)) - transform.position;
	}
	
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	public void AddCurve () {
		Vector3 point = getpoint(numpoints - 1);
		Array.Resize(ref _points, numpoints + 3);
		point.x += 1f;
        setpoint(numpoints - 3,point);
		point.x += 1f;
        setpoint(numpoints - 2,point);
        point.x += 1f;
        setpoint(numpoints - 1,point);

		Array.Resize(ref modes, modes.Length + 1);
		modes[modes.Length - 1] = modes[modes.Length - 2];
		EnforceMode(numpoints - 4);

		if (loop) {
			setpoint(numpoints - 1,getpoint(0));
			modes[modes.Length - 1] = modes[0];
			EnforceMode(0);
		}
	}
	
	public void Reset () {
		_points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
		modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
	}
}