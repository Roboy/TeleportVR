using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;
#if SENSEGLOVE
using SenseGloveCs.Kinematics;
using SG;
using SG.Calibration;
#endif

namespace Training.Calibration
{
    // This class manages the SenseGlove calibration for a single hand
    public class HandCalibrator : MonoBehaviour
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
        public Animator handAnimator;
        public GameObject hudElements;

        public float maxError;
        [Tooltip("Time to hold each calibration step for (seconds)")]
        public float dwellTime;
        [Tooltip("Time to wait before dwellig on each calibration (seconds)")]
        public float waitTime;
        [Tooltip("Time to wait show each notification for (seconds)")]
        public bool isRight = true;
        public bool calibrating = false;
        public Completion completionWidget;
        public AudioClip handOpen, handClosed, fingersExt, fingersFlexed, thumbUp, thumbFlex, AbdOut, NoThumbAbd;


        private SG_SenseGloveHardware hand;

        private InterpolationSet_IMU interpolator;
        private CalibrationPose[] poses;

        private Pose currentPose = Pose.HandOpen;
        public Step currentStep = Step.ShowInstruction;

        private readonly PoseBuffer poseStore = new PoseBuffer();

        private Timer dwellTimer;
        private Timer waitTimer;

        private readonly List<System.Action<Step>> doneCallbacks = new List<System.Action<Step>>();

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

        #region Setup

        // Start is called before the first frame update
        void Start()
        {
            // find SenseGlove hardware automatically, as scene cross-referencing is not supported in unity
            foreach (SG.SG_SenseGloveHardware obj in GameObject.FindObjectsOfType(typeof(SG.SG_SenseGloveHardware)))
            {
                string connectionMethod = obj.connectionMethod.ToString().ToLower();
                bool match = isRight ? connectionMethod.Equals("nextrighthand") : connectionMethod.Equals("nextlefthand");
                if (match)
                {
                    hand = obj;
                }
            }

            if (hand == null)
            {
                throw new MissingComponentException($"Could not find {(isRight ? "right" : "left")} SG_SenseGloveHardware in Scene.");
            }

            waitTimer = new Timer();
            // advance to Dwell after wait
            waitTimer.SetTimer(waitTime, WaitDone);

            dwellTimer = new Timer();
            dwellTimer.SetTimer(dwellTime, DwellDone);

            Debug.Log($"Awaiting connection with {(isRight ? "right" : "left")} SenseGlove... ");
        }
        #endregion

        private void ShowInstruction()
        {
            Debug.Log($"Please move your hands in pose: {currentPose}");
            handAnimator.SetInteger("handState", (int)currentPose);
            currentStep = Step.Wait;
            string right = isRight ? "right" : "left";
            switch (currentPose)
            {
                case Pose.HandOpen:
                    TutorialSteps.PublishNotification($"Open {right} your hand", waitTime + dwellTime);
                    break;
                case Pose.HandClosed:
                    TutorialSteps.PublishNotification($"Make a fist with your {right} hand", waitTime + dwellTime);
                    break;
                case Pose.FingersExt:
                    TutorialSteps.PublishNotification($"Extend your {right} fingers", waitTime + dwellTime);
                    break;
                case Pose.FingersFlexed:
                    TutorialSteps.PublishNotification($"Flex your {right} fingers", waitTime + dwellTime);
                    break;
                case Pose.ThumbUp:
                    TutorialSteps.PublishNotification("Give me a thumbs up", waitTime + dwellTime);
                    break;
                case Pose.ThumbFlex:
                    TutorialSteps.PublishNotification($"Flex your {right} thumb", waitTime + dwellTime);
                    break;
                case Pose.AbdOut:
                    TutorialSteps.PublishNotification($"Abduct {right} you thumb", waitTime + dwellTime);
                    break;
                case Pose.NoThumbAbd:
                    TutorialSteps.PublishNotification($"Move your {right} thumb up", waitTime + dwellTime);
                    break;
            }
        }

        private void WaitDone()
        {
            waitTimer.ResetTimer();
            currentStep = Step.Dwell;
            Debug.Log("wait done");
        }

        /// <summary>
        /// When Dwell time is over, save the calibration pose
        /// </summary>
        private void DwellDone()
        {

            completionWidget.active = false;
            // make sure this is not called too often
            dwellTimer.ResetTimer();

            poseStore.Clear();

            // calibrate the pose
            Vector3[] handValues = GetCalibrationValues(hand);
            poses[(int)currentPose].CalibrateParameters(handValues, ref interpolator);

            // update the glove's interpolation profiles
            hand.SetInterpolationProfile(interpolator);
            Debug.Log($"Calibrated Pose {currentPose}");

            currentPose += 1;
            currentStep = Step.ShowInstruction;

            // if all are calibrated save
            const int maxPose = (int)(Pose.NoThumbAbd) + 1;
            if ((int)currentPose >= maxPose)
            {
                currentStep = Step.Done;
            }
        }

        public void StartCalibration()
        {
            calibrating = true;
        }

        public void StopCalibration()
        {
            calibrating = false;
            dwellTimer.ResetTimer();
            waitTimer.ResetTimer();
        }

        // Update is called once per frame
        void Update()
        {
            if (interpolator == null &&
                hand.IsLinked &&
                hand.GetInterpolationProfile(out interpolator))
            {
                poses = LoadProfiles(interpolator);
                Debug.Log($"Connected {(isRight ? "right" : "left")} Hand");
            }
            // only show HUD elements if calibration is active
            hudElements.SetActive(calibrating);

            if (calibrating)
            {
                switch (currentStep)
                {
                    // 1. show instruction
                    case Step.ShowInstruction:
                        ShowInstruction();
                        break;
                    // 2. wait
                    case Step.Wait:
                        waitTimer.LetTimePass(Time.deltaTime);
                        poseStore.Clear();
                        break;
                    // 3. dwell
                    case Step.Dwell:
                        dwellTimer.LetTimePass(Time.deltaTime);
                        completionWidget.active = true;
                        completionWidget.progress = dwellTimer.GetFraction();

                        poseStore.AddPose(GetCalibrationValues(hand));
                        float error = poseStore.ComputeError();

                        if (error > maxError)
                        {
                            const string warningText = "Finger position changed too much, retry";
                            Debug.LogWarning(warningText);

                            //// show error message in Toastr
                            //if (!shownRetryMessage)
                            //{
                            //    ToastrWidget notificationWidget = (ToastrWidget)Manager.Instance.FindWidgetWithID(10);
                            //    RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, warningText, dwellTime, new byte[] { 255, 40, 15, 255 });
                            //    notificationWidget.ProcessRosMessage(toastrMessage);
                            //}

                            poseStore.Clear();
                            dwellTimer.ResetTimer();
                        }
                        break;
                    // 4. calibrate Step
                    case Step.Done:

                        hand.SaveHandCalibration();
                        Debug.Log("Saved Calibration Profiles");
                        calibrating = false;

                        foreach (var callback in doneCallbacks)
                        {
                            callback(currentStep);
                        }
                        return;
                    default: break;
                }
            }
        }

        //#endif

        /// <summary>
        /// The given callback is called, once the calibration for this hand is done
        /// </summary>
        /// <param name="callback"></param>
        public void OnDone(System.Action<Step> callback)
        {
            doneCallbacks.Add(callback);
        }
    }



}

