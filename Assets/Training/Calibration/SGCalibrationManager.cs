using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if SENSEGLOVE
using SenseGloveCs.Kinematics;
using SG;
using SG.Calibration;
//#endif

// This class manages the SenseGlove calibration
public class SGCalibrationManager : MonoBehaviour
{
    // <summary> Calibration poses, used to access SG_CalPoses in an array. </summary>
    public enum Pose
    {
        HandOpen = 0,
        HandClosed,
        FingersExt,
        FingersFlexed,
        ThumbUp,
        ThumbFlex,
        AbdOut,
        NoThumbAbd,
    }

    public enum Step
    {
        ShowInstruction = 0,
        Wait,
        Dwell,
        Done
    }

    //#if SENSEGLOVE
    public Animator rightHandAnimator;
    public Animator leftHandAnimator;

    public Widgets.TextView textView;
    public float maxError;
    [Tooltip("Time to hold each calibration step for (seconds)")]
    public float dwellTime;
    [Tooltip("Time to wait before dwellig on each calibration (seconds)")]
    public float waitTime;



    protected SG_SenseGloveHardware rightHand;
    protected SG_SenseGloveHardware leftHand;

    private InterpolationSet_IMU rightInterpolator;
    private InterpolationSet_IMU leftInterpolator;
    private CalibrationPose[] rightPoses;
    private CalibrationPose[] leftPoses;

    private Pose currentPose = Pose.HandOpen;
    private Step currentStep = Step.ShowInstruction;

    private readonly PoseBuffer leftPoseStore = new PoseBuffer();
    private readonly PoseBuffer rightPoseStore = new PoseBuffer();

    private Timer dwellTimer;
    private Timer waitTimer;




    private CalibrationPose[] LoadProfiles(InterpolationSet_IMU interpolator)
    {
        // order in this array needs to match the one in enum Pose
        return new CalibrationPose[]
        {
             CalibrationPose.GetFullOpen(ref interpolator),
             CalibrationPose.GetFullFist(ref interpolator),
             CalibrationPose.GetOpenHand(ref interpolator),
             CalibrationPose.GetFist(ref interpolator),
             CalibrationPose.GetThumbsUp(ref interpolator),
             CalibrationPose.GetThumbFlexed(ref interpolator),
             CalibrationPose.GetThumbAbd(ref interpolator),
             CalibrationPose.GetThumbNoAbd(ref interpolator)
        };
    }
    /// <summary> Get Calibration Values from the hardware, as the interpolation solver would. </summary>
    /// <returns></returns>
    public Vector3[] GetCalibrationValues(SG_SenseGloveHardware hand)
    {
        float[][] rawAngles = hand.GloveData.gloveValues;
        float[][] Nsensors = Interp4Sensors.NormalizeAngles(rawAngles);
        Vect3D[][] inputAngles = SenseGloveModel.ToGloveAngles(Nsensors);

        Vector3[] res = new Vector3[5];
        for (int f = 0; f < inputAngles.Length; f++)
        {
            res[f] = Vector3.zero;
            for (int j = 0; j < inputAngles.Length; j++)
            {
                res[f] += new Vector3(inputAngles[f][j].x, inputAngles[f][j].y, inputAngles[f][j].z);
            }
        }
        return res;
    }


    // Start is called before the first frame update
    void Start()
    {
        // find SenseGlove hardware automatically, as scene cross-referencing is not supported in unity
        foreach (SG.SG_SenseGloveHardware obj in GameObject.FindObjectsOfType(typeof(SG.SG_SenseGloveHardware)))
        {
            string connectionMethod = obj.connectionMethod.ToString().ToLower();
            if (connectionMethod.Equals("nextrighthand"))
            {
                rightHand = obj;
            }
            else if (connectionMethod.Equals("nextlefthand"))
            {
                leftHand = obj;
            }
        }
        if (rightHand == null || leftHand == null)
        {
            throw new MissingComponentException($"Could not find SG_SenseGloveHardware in Scene. right: {rightHand}, left: {leftHand}");
        }

        waitTimer = new Timer();
        // advance to Dwell after wait
        waitTimer.SetTimer(waitTime, WaitDone);

        dwellTimer = new Timer();
        dwellTimer.SetTimer(dwellTime, DwellDone);

        Debug.Log("Awaiting connection with SenseGlove... ");
    }

    private void ShowInstruction()
    {
        Debug.Log($"Please move your hands in pose: {currentPose}");
        rightHandAnimator.SetInteger("rightHandState", (int)currentPose);
        currentStep = Step.Wait;
    }

    private void WaitDone()
    {
        waitTimer.ResetTimer();
        currentStep = Step.Dwell;
        Debug.Log("Waiting done");
    }

    /// <summary>
    /// When Dwell time is over, save the calibration pose
    /// </summary>
    private void DwellDone()
    {
        // make sure this is not called too often
        dwellTimer.ResetTimer();

        rightPoseStore.Clear();
        leftPoseStore.Clear();

        Vector3[] rightHandValues = GetCalibrationValues(rightHand);
        Vector3[] leftHandValues = GetCalibrationValues(leftHand);

        // calibrate the pose
        rightPoses[(int)currentPose].CalibrateParameters(rightHandValues, ref rightInterpolator);
        leftPoses[(int)currentPose].CalibrateParameters(leftHandValues, ref leftInterpolator);

        // update the glove's interpolation profiles
        rightHand.SetInterpolationProfile(rightInterpolator);
        leftHand.SetInterpolationProfile(leftInterpolator);
        Debug.Log($"Calibrated Pose {currentPose}");

        currentPose += 1;
        currentStep = Step.ShowInstruction;

        // if all are calibrated save
        const int maxPose = (int)(Pose.NoThumbAbd) + 1;
        if ((int)currentPose >= maxPose)
        {
            rightHand.SaveHandCalibration();
            leftHand.SaveHandCalibration();
            Debug.Log("Saved Calibration Profiles");
            currentStep = Step.Done;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rightInterpolator == null &&
            rightHand.IsLinked &&
            rightHand.GetInterpolationProfile(out rightInterpolator))
        {
            rightPoses = LoadProfiles(rightInterpolator);
            Debug.Log("Connected Right Hand");
        }
        if (leftInterpolator == null &&
            leftHand.IsLinked &&
            leftHand.GetInterpolationProfile(out leftInterpolator))
        {
            leftPoses = LoadProfiles(leftInterpolator);
            Debug.Log("Connected Left Hand");
        }

        // only continue of both gloves have been connected
        if (leftInterpolator == null || rightInterpolator == null)
        {
            return;
        }


        // 1. show instruction
        // 2. wait
        // 3. dwell
        // 4. calibrate Step

        switch (currentStep)
        {
            case Step.ShowInstruction:
                ShowInstruction();
                break;
            case Step.Wait:
                waitTimer.LetTimePass(Time.deltaTime);
                rightPoseStore.Clear();
                leftPoseStore.Clear();
                break;
            case Step.Dwell:
                dwellTimer.LetTimePass(Time.deltaTime);

                rightPoseStore.AddPose(GetCalibrationValues(rightHand));
                leftPoseStore.AddPose(GetCalibrationValues(leftHand));

                float rightError = rightPoseStore.ComputeError();
                float leftError = leftPoseStore.ComputeError();

                if (rightError > maxError || leftError > maxError)
                {
                    Debug.Log($"Deviation too large: (r:{rightError}, l:{leftError}), try to calibrate pose {currentPose} again.");
                    rightPoseStore.Clear();
                    leftPoseStore.Clear();
                    dwellTimer.ResetTimer();
                }
                break;
            case Step.Done:
                return;
            default: break;
        }

    }
    //#endif
}
