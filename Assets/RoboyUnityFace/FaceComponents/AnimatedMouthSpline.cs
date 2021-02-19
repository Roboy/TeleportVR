using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class AnimatedMouthSpline : BezierSpline {
    BezierSpline spline;
    //public Animator

    public Vector3 MouthLeftPosition = new Vector3(-2,0,0);
    public Vector3 MouthRightPosition = new Vector3(-1, 0, 0);
    public Vector3 MouthLeftControl = new Vector3(1, 0, 0);
    public Vector3 MouthRightControl = new Vector3(2, 0, 0);

    protected override Vector3 getpoint(int id)
    {
        switch (id)
        {
            case 0: return MouthLeftPosition;
            case 1: return MouthLeftControl;
            case 2: return MouthRightPosition;
            default:
            case 3: return MouthRightControl;
        }
    }

    protected override void setpoint(int id, Vector3 val)
    {
        switch (id)
        {
            case 0: MouthLeftPosition = val; break;
            case 1: MouthLeftControl = val; break;
            case 2: MouthRightPosition = val; break;
            default:
            case 3: MouthRightControl = val; break;
        }
    }

    protected override int numpoints { get { return 4; } }

}
