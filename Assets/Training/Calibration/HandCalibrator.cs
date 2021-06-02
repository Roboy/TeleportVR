using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widgets;

using SenseGloveCs.Kinematics;
using SG;
using SG.Calibration;


namespace Training.Calibration
{

    // This class manages the SenseGlove calibration for a single hand
    public class HandCalibrator : MonoBehaviour
    {
        public enum Step
        {
            ShowInstruction = 0,
            Wait,
            Dwell,
            TestInit,
            Test,
            Done,
        }

        public enum Pose
        {
            HandOpen = 0,
            HandClosed = 1,
            //FingersExt = 2,
            //FingersFlexed = 3,
            ThumbUp = 4,
            ThumbFlex = 5,
            AbdOut = 6,
            //NoThumbAbd = 7,
        }

        [System.Serializable]
        public class Calibration
        {
            [Tooltip("Maximum deviation from a held hand pose to tolerate during dwell time. (m^2)")]
            public float maxError = 0.001f;
            [Tooltip("Time to wait before dwellig on each calibration (seconds)")]
            public float waitTime = 5;
            [Tooltip("Time to hold each calibration step for (seconds)")]
            public float dwellTime = 3;

            // not visibe in the inspector
            protected internal Timer waitTimer = new Timer();
            protected internal Timer dwellTimer = new Timer();

        }

        [System.Serializable]
        public class Test
        {
            [Tooltip("Maximum deviation from a held hand pose to tolerate during dwell time. (m^2)")]
            public float maxError = 0.5f;
            [Tooltip("Time to hold each calibration step for (seconds)")]
            public float dwellTime = 3;

            // not visibe in the inspector
            protected internal Timer dwellTimer = new Timer();
        }

        public Calibration calibrationParams;
        public Test testParams;

        //#if SENSEGLOVE
        public bool isRight = true;
        public Animator handAnimator;
        public GameObject virtualHand;
        public Completion completionWidget;

        public AudioClips.SGHand audioClips;

        // inspector display variables
        public Step currentStep = Step.ShowInstruction;
        public bool calibrating = false;

        private SG_SenseGloveHardware hand;
        private InterpolationSet_IMU interpolator;
        private CalibrationPose[] poses;

        // size is max Pose index
        private readonly Vector3[][] poseValues = new Vector3[8][];
        private readonly PoseBuffer poseStore = new PoseBuffer();

        private Pose currentPose = Pose.HandOpen;

        private readonly List<System.Action<Step>> doneCallbacks = new List<System.Action<Step>>();

        private string lrName
        {
            get { return isRight ? "right" : "left"; }
        }

        private CalibrationPose[] LoadProfiles()
        {
            if (!hand.IsLinked || interpolator == null)
            {
                throw new UnassignedReferenceException("Cannot load profiles for disconnected SenseGlove");
            }
            // order in this array needs to match the one in the Enum Pose
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
        public Vector3[] GetCalibrationValues()
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
                throw new MissingComponentException($"Could not find {lrName} SG_SenseGloveHardware in Scene.");
            }


            // calibration timers
            calibrationParams.waitTimer.SetTimer(calibrationParams.waitTime, CalibrationWaitDone);
            calibrationParams.dwellTimer.SetTimer(calibrationParams.dwellTime, CalibrationDwellDone);

            // test Timer
            testParams.dwellTimer.SetTimer(testParams.dwellTime, () => currentStep = Step.Done);
            virtualHand.SetActive(false);
            calibrating = false;
            Debug.Log($"Awaiting connection with {lrName} SenseGlove... ");
        }
        #endregion

        private void ShowInstruction()
        {
            string right = isRight ? "right" : "left";
            Debug.Log($"Calibrating pose {currentPose} for {right} hand");
            handAnimator.SetInteger("handState", (int)currentPose);
            currentStep = Step.Wait;
            switch (currentPose)
            {
                case Pose.HandOpen:
                    TutorialSteps.Instance.ScheduleAudioClip(audioClips.handOpen, queue: false);
                    SendToast($"Open {right} your hand", calibrationParams.waitTime + calibrationParams.dwellTime);
                    break;
                case Pose.HandClosed:
                    TutorialSteps.Instance.ScheduleAudioClip(audioClips.handClosed, queue: false);
                    SendToast($"Make a fist with your {right} hand", calibrationParams.waitTime + calibrationParams.dwellTime);
                    break;
                //case Pose.FingersExt:
                //    TutorialSteps.Instance.ScheduleAudioClip(fingersExt, queue: false);
                //    SendToast($"Extend your {right} fingers", waitTime + dwellTime);
                //    break;
                //case Pose.FingersFlexed:
                //    TutorialSteps.Instance.ScheduleAudioClip(fingersFlexed, queue: false);
                //    SendToast($"Flex your {right} fingers", waitTime + dwellTime);
                //    break;
                case Pose.ThumbUp:
                    TutorialSteps.Instance.ScheduleAudioClip(audioClips.thumbUp, queue: false);
                    SendToast("Give me a thumbs up", calibrationParams.waitTime + calibrationParams.dwellTime);
                    break;
                case Pose.ThumbFlex:
                    TutorialSteps.Instance.ScheduleAudioClip(audioClips.thumbFlex, queue: false);
                    SendToast($"Flex your {right} thumb", calibrationParams.waitTime + calibrationParams.dwellTime);
                    break;
                case Pose.AbdOut:
                    TutorialSteps.Instance.ScheduleAudioClip(audioClips.abdOut, queue: false);
                    SendToast($"Move your {right} thumb out", calibrationParams.waitTime + calibrationParams.dwellTime);
                    break;
                //case Pose.NoThumbAbd:
                //    TutorialSteps.Instance.ScheduleAudioClip(noThumbAbd, queue: false);
                //    SendToast($"Move your {right} thumb up", waitTime + dwellTime);
                //    break;
                default:
                    Debug.LogError($"Pose unknown: {currentPose}");
                    break;
            }
        }

        private void CalibrationWaitDone()
        {
            calibrationParams.waitTimer.ResetTimer();
            currentStep = Step.Dwell;
            Debug.Log("Calibration wait done");
        }

        /// <summary>
        /// When Dwell time is over, save the calibration pose
        /// </summary>
        private void CalibrationDwellDone()
        {
            // hide completion widget
            completionWidget.active = false;

            calibrationParams.dwellTimer.ResetTimer();
            poseStore.Clear();

            // calibrate the pose
            Vector3[] handValues = GetCalibrationValues();
            poseValues[(int)currentPose] = handValues;
            poses[(int)currentPose].CalibrateParameters(handValues, ref interpolator);

            // update the glove's interpolation profiles
            hand.SetInterpolationProfile(interpolator);
            Debug.Log($"Calibrated Pose {currentPose}");

            // if all are calibrated move to test state
            if (currentPose.IsLast())
            {
                currentStep = Step.TestInit;
            }
            else
            {
                currentStep = Step.ShowInstruction;
                currentPose = currentPose.Next();
            }
        }

        public void StartCalibration()
        {
            // calibration starts again in the ShowInstruction Step
            Debug.Log("start calibration");
            currentStep = Step.ShowInstruction;
            calibrating = true;
        }


        public void PauseCalibration()
        {
            Debug.Log("pause calibration");
            calibrating = false;
            calibrationParams.dwellTimer.ResetTimer();
            calibrationParams.waitTimer.ResetTimer();
            testParams.dwellTimer.ResetTimer();
        }

        // Update is called once per frame
        void Update()
        {
            // load old calibration if existing
            if (interpolator == null &&
                hand.IsLinked &&
                hand.GetInterpolationProfile(out interpolator))
            {
                poses = LoadProfiles();
                Debug.Log($"Connected {(isRight ? "right" : "left")} Hand");
            }

            // only start the calibration, if the SenseGlove could be found and the calibrating flag was set
            if (calibrating && hand.IsLinked)
            {
                switch (currentStep)
                {
                    // 1. show instruction
                    case Step.ShowInstruction:
                        {
                            virtualHand.SetActive(true);
                            ShowInstruction();
                            break;
                        }
                    // 2. wait
                    case Step.Wait:
                        {
                            calibrationParams.waitTimer.LetTimePass(Time.deltaTime);
                            poseStore.Clear();
                            break;
                        }
                    // 3. dwell
                    case Step.Dwell:
                        {
                            if (TutorialSteps.Instance.IsAudioPlaying())
                            {
                                break;
                            }
                            calibrationParams.dwellTimer.LetTimePass(Time.deltaTime);
                            completionWidget.active = true;
                            completionWidget.text = "calibrating";
                            completionWidget.progress = calibrationParams.dwellTimer.GetFraction();

                            poseStore.AddPose(GetCalibrationValues());
                            float error = poseStore.ComputeError();
                            if (error > calibrationParams.maxError)
                            {
                                const string warningText = "Finger position changed too much, retry";
                                Debug.LogWarning(warningText);

                                poseStore.Clear();
                                calibrationParams.dwellTimer.ResetTimer();
                            }
                            break;

                        }
                    // 3.5 finish calibration & init testing phase
                    case Step.TestInit:
                        {
                            hand.SaveHandCalibration();
                            virtualHand.SetActive(false);
                            Debug.Log($"Saved Calibration Profiles for {lrName} hand");

                            TutorialSteps.Instance.ScheduleAudioClip(audioClips.test, queue: false);
                            SendToast($"{lrName} thums up to continue", duration: 2 * testParams.dwellTime);

                            currentStep = Step.Test;
                            goto case Step.Test;
                        }
                    // 4. test calibration
                    case Step.Test:
                        {
                            if (TutorialSteps.Instance.IsAudioPlaying())
                            {
                                break;
                            }
                            PoseBuffer buffer = new PoseBuffer(bufferSize: 2);
                            buffer.AddPose(poseValues[(int)Pose.ThumbUp]);
                            buffer.AddPose(GetCalibrationValues());
                            float error = buffer.ComputeError();

                            Debug.Log($"Thumbs Up error {error}");
                            testParams.dwellTimer.LetTimePass(Time.deltaTime);
                            if (error > testParams.maxError)
                            {
                                testParams.dwellTimer.ResetTimer();
                            }

                            completionWidget.text = "hold";
                            completionWidget.active = true;
                            completionWidget.progress = testParams.dwellTimer.GetFraction();
                            break;
                        }
                    // 5. calibration done
                    case Step.Done:
                        {
                            calibrating = false;
                            completionWidget.active = false;
                            foreach (var callback in doneCallbacks)
                            {
                                callback(currentStep);
                            }
                            break;

                        }
                    default: break;
                }
            }
        }

        //#endif

        /// <summary>
        /// The given callback is called, once the calibration for this hand is done
        /// </summary>
        /// <param name="callback">called, when the calibration is done</param>
        public void OnDone(System.Action<Step> callback)
        {
            doneCallbacks.Add(callback);
        }

        private bool SendToast(string message, float duration = 5)
        {
            if (duration == 0)
            {
                duration = calibrationParams.dwellTime;
            }
            byte[] color = new byte[] { 255, 40, 15, 255 };

            ToastrWidget widget = (ToastrWidget)Manager.Instance.FindWidgetWithID(10);
            RosJsonMessage msg = RosJsonMessage.CreateToastrMessage(10, message, duration, color);
            bool oldMessage = false;
            foreach (var template in widget.toastrActiveQueue)
            {
                if (template.toastrMessage == message && template.toastrDuration == duration)
                {
                    oldMessage = true;
                }
            }
            if (!oldMessage)
            {
                widget.ProcessRosMessage(msg);
            }
            return !oldMessage;
        }
    }
}

