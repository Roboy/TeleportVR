using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public BioIK.BioIK rightHand;
    public BioIK.BioIK leftHand;
    public int minMotorStep = 0;
    public int maxMotorStep = 800;
    //public AnimationCurve jointSpaceMap = new AnimationCurve(new Keyframe(0f, 0f, -1f, -1f), new Keyframe(1f, 1f, 1f, 1f));


    // can only track up to two joints per set, if more are required, update the code in Start()
    private readonly List<List<string>> jointSets = new List<List<string>> {
                new List<string> {"rh_FFJ3", "rh_FFJ2"},
                new List<string> {"rh_MFJ3", "rh_MFJ2"},
                new List<string> {"rh_RFJ3", "rh_RFJ2"},
                new List<string> {"rh_THJ4"},
                new List<string> {"lh_FFJ3", "lh_FFJ2"},
                new List<string> {"lh_MFJ3", "lh_MFJ2"},
                new List<string> {"lh_RFJ3", "lh_RFJ2"},
                new List<string> {"lh_THJ4"},

            };
    private List<float> jointRanges = new List<float>();
    private List<Vector2> jointMinVectors = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        // calculate Angle range for every BioIK joint
        Dictionary<string, float> minAngles = new Dictionary<string, float>();
        Dictionary<string, float> maxAngles = new Dictionary<string, float>();
        foreach (var hand in new BioIK.BioIK[] { rightHand, leftHand })
        {
            foreach (var segment in hand.Segments)
            {
                if (segment.Joint == null) continue;
                minAngles[segment.name] = (float)segment.Joint.X.LowerLimit;
                maxAngles[segment.name] = (float)segment.Joint.X.UpperLimit;
            }
        }
        foreach (var joint in jointSets)
        {
            Vector2 min, max;
            if (joint.Count == 1)
            {
                min = new Vector2(minAngles[joint[0]], 0f);
                max = new Vector2(maxAngles[joint[0]], 0f);
            }
            else if (joint.Count == 2)
            {
                min = new Vector2(minAngles[joint[0]], minAngles[joint[1]]);
                max = new Vector2(maxAngles[joint[0]], maxAngles[joint[1]]);
            }
            else
            {
                throw new System.ArgumentException($"jointSets can only have one, or two tracked joints, got: {joint.Count}");
            }
            jointMinVectors.Add(min);
            jointRanges.Add((max - min).magnitude);
        }
    }

    // Right hand: 
    // rh_FF [0,1] 
    // rh_MF [0,1]
    // rh_RF [0,1]
    // rh_TH [0,1]
    // Left hand:
    // lh_FF [0,1]
    // lh_MF [0,1]
    // lh_RF [0,1]
    // lh_TH [0,1]
    public List<float> GetMotorPositions()
    {

        Dictionary<string, float> currentAngles = new Dictionary<string, float>();
        foreach (var hand in new BioIK.BioIK[] { rightHand, leftHand })
        {
            foreach (var segment in hand.Segments)
            {
                if (segment.Joint == null) continue;
                currentAngles[segment.name] = (float)segment.Joint.X.CurrentValue;
            }
        }

        List<float> motorPos = new List<float>();
        for (int i = 0; i < jointSets.Count; i++)
        {
            List<string> joints = jointSets[i];
            Vector2 current;
            if (joints.Count == 1)
            {
                current = new Vector2(currentAngles[joints[0]], 0f);
            }
            else
            {
                current = new Vector2(currentAngles[joints[0]], currentAngles[joints[1]]);
            }
            float dist = (current - jointMinVectors[i]).magnitude;
            float range = jointRanges[i];
            motorPos.Add(dist / range);

        }
        //Debug.Log($"motor_set() motorAngles = [{ string.Join(", ", motorPos.ConvertAll(x => x.ToString()).ToArray())}]");
        return motorPos;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
