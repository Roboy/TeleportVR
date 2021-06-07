using System.Collections.Generic;
using UnityEngine;
using Widgets;

namespace Training
{
    public class TutorialSteps : MonoBehaviour//Singleton<TutorialSteps>
    {
        public static TutorialSteps Instance;

        public TrainingStep currentStep;

        public AudioClips.SGTraining senseGloveAudio;
        public AudioClips.Controller controllerAudio;
        public AudioClips.DriveJoystick driveJoystickAudio;
        public AudioClips.Misc miscAudio;

        public List<AudioClip> praisePhrases = new List<AudioClip>();
        public AudioSource[] audioSourceArray;
        public AudioSource sirenAudioSource;
        public bool waitingForNod = false;
        public Calibration.HandCalibrator rightCalibrator, leftCalibrator;

        int toggle;
        double prevDuration = 0.0;
        double prevStart = 0.0;
        TrainingStep lastCorrectedAtStep = TrainingStep.IDLE;
        bool trainingStarted = false, startTraining = true;

        public enum TrainingStep
        {
            IDLE,
            HEAD,
            LEFT_ARM,
            LEFT_HAND,
            RIGHT_ARM,
            RIGHT_HAND,
            WHEELCHAIR,
            DONE
        }

        [SerializeField] private Transform handCollectables;

        [SerializeField] private GameObject designatedArea;


        void Start()
        {
            Debug.Log(StateManager.Instance.TimesStateVisited(StateManager.States.Training));
            // get a reference to this singleton, as scripts from other scenes are not able to do this
            _ = Instance;
            if (StateManager.Instance.TimesStateVisited(StateManager.States.Training) <= 1)
            {
                ScheduleAudioClip(miscAudio.welcome, queue: true, delay: 1.0);
                ScheduleAudioClip(miscAudio.imAria, queue: true);//, delay: 2.0);

                PublishNotification("Welcome to Teleport VR!"); //\n" +
                                                                //"Take a look around. " +
                                                                //"In the mirror you can see how you are controlling the Head of Roboy.\n" +
                                                                //"Look at the blue sphere to get started!");
                PublishNotification("I am Aria - your personal telepresence trainer.");

            }
            else
            {
                Debug.Log("Training routine skipped.");
                startTraining = false;
            }
            //currentStep = TrainingStep.RIGHT_HAND;
            //NextStep();
            //trainingStarted = false;
        }

        public void ScheduleAudioClip(AudioClip clip, bool queue = false, double delay = 0)
        {
            var timeLeft = 0.0;
            //queue = false;
            if (IsAudioPlaying() && queue)
            {
                timeLeft = prevDuration - (AudioSettings.dspTime - prevStart);
                if (timeLeft > 0) delay = timeLeft;
            }


            if (queue) toggle = 1 - toggle;
            audioSourceArray[toggle].clip = clip;
            //if (queue)
            //    prevStart = AudioSettings.dspTime + prevDuration + delay;
            //else
            prevStart = AudioSettings.dspTime + delay;
            audioSourceArray[toggle].PlayScheduled(prevStart);

            //if (queue)
            //    audioSourceArray[toggle].PlayScheduled(AudioSettings.dspTime + prevDuration + delay);
            //else
            //    audioSourceArray[toggle].PlayScheduled(AudioSettings.dspTime + delay);
            prevDuration = (double)clip.samples / clip.frequency;

        }


        /// <summary>
        /// Shows a message on the notification widget
        /// </summary>
        /// <param name="message"></param>
        public static bool PublishNotification(string message, float duration = 5f)
        {
            byte[] color = new byte[] { 255, 40, 15, 255 };
            ToastrWidget widget = (ToastrWidget)Manager.Instance.FindWidgetWithID(10);
            RosJsonMessage msg = RosJsonMessage.CreateToastrMessage(10, message, duration, color);

            bool isOld = false;
            foreach(var template in widget.toastrActiveQueue)
            {
                if (template.toastrMessage == message && template.toastrDuration == duration)
                {
                    isOld = true;
                    break;
                }
            }
            if (!isOld)
            {
                widget.ProcessRosMessage(msg);
            }
            // published?
            return !isOld;
        }

        public void PraiseUser()
        {
            Debug.Log("Praise");
            ScheduleAudioClip(praisePhrases[Random.Range(0, praisePhrases.Count)]);
        }

        public void CorrectUser(string correctButton)
        {
            AudioClip audio;
            switch (correctButton)
            {
                case "tigger":
                    audio = miscAudio.wrongTrigger;
                    break;
                case "grip":
                    audio = miscAudio.wrongGrip;
                    break;
                default:
                    audio = miscAudio.wrongButton;
                    break;
            }
            Debug.Log("Correcting User");
            if (lastCorrectedAtStep != currentStep && (currentStep == TrainingStep.LEFT_ARM || currentStep == TrainingStep.RIGHT_ARM))
            {
                ScheduleAudioClip(miscAudio.wrongTrigger);
                lastCorrectedAtStep = currentStep;
            }
        }

        /// <summary>
        /// Continues to the next step in the Tutorial
        /// </summary>
        /// <param name="praise"></param>
        public void NextStep(bool praise = false)
        {
            //if (praise)
            //    PraiseUser();
            currentStep++;
            Debug.Log("current tutorial step: " + currentStep);
            switch (currentStep)
            {
                case TrainingStep.HEAD:
                    ScheduleAudioClip(miscAudio.head);
                    PublishNotification("Try moving your head around");
                    ScheduleAudioClip(miscAudio.nod, delay: 0);
                    waitingForNod = true;
                    break;
                case TrainingStep.LEFT_ARM:
#if SENSEGLOVE
                    ScheduleAudioClip(senseGloveAudio.leftArm, queue: true);
                    ScheduleAudioClip(senseGloveAudio.leftBall, queue: true);
                    PublishNotification("Move youre left arm and try to touch the blue ball");
                    var colTF = PlayerRig.Instance.transform.position;
                    colTF.y -= 0.1f;
                    colTF.z += 0.2f;
                    handCollectables.transform.position = colTF;
                    handCollectables.Find("HandCollectableLeft").gameObject.SetActive(true);
#else
                    ScheduleAudioClip(controllerAudio.leftArm, queue: true);
                    ScheduleAudioClip(controllerAudio.leftBall, queue: true);
                    PublishNotification("Press and hold the index trigger and try moving your left arm");
                    var colTF = PlayerRig.Instance.transform.position;
                    colTF.y -= 0.1f;
                    colTF.z += 0.2f;
                    handCollectables.transform.position = colTF;
                    handCollectables.Find("HandCollectableLeft").gameObject.SetActive(true);
#endif
                    break;
                case TrainingStep.LEFT_HAND:
#if SENSEGLOVE

                    PublishNotification("Move your left hand into the blue box");
                    ScheduleAudioClip(senseGloveAudio.leftHandStart);
                    leftCalibrator.OnDone(step => NextStep());
#else
                    ScheduleAudioClip(controllerAudio.leftHand, queue: true, delay: 0);
                    PublishNotification("Press the grip button on the side to close the hand.");
#endif
                    break;
                case TrainingStep.RIGHT_ARM:
#if SENSEGLOVE
                    // force stop the calibration, if not done so already
                    leftCalibrator.PauseCalibration();

                    ScheduleAudioClip(senseGloveAudio.rightArm);
                    ScheduleAudioClip(senseGloveAudio.rightBall, queue: true);
                    PublishNotification("Move your right arm and try to touch the blue ball");
                    handCollectables.Find("HandCollectableRight").gameObject.SetActive(true);
#else
                    ScheduleAudioClip(controllerAudio.rightArm);
                    ScheduleAudioClip(controllerAudio.rightBall, queue: true);
                    PublishNotification("Press and hold the index trigger and try moving your right arm");
                    //PublishNotification("To move your arm, hold down the hand trigger on the controller with your middle finger.");
                    handCollectables.Find("HandCollectableRight").gameObject.SetActive(true);
#endif
                    break;
                case TrainingStep.RIGHT_HAND:
#if SENSEGLOVE
                    PublishNotification("Move your right hand into the blue box");
                    ScheduleAudioClip(senseGloveAudio.rightHandStart);
                    rightCalibrator.OnDone(step => NextStep());
#else
                    ScheduleAudioClip(controllerAudio.rightHand, queue: true, delay: 0);
                    PublishNotification("Press the grip button to close the hand.");
#endif
                    break;
                case TrainingStep.WHEELCHAIR:

                    ScheduleAudioClip(driveJoystickAudio.drive, delay: 1);
                    //ScheduleAudioClip(emergency, queue: true);

                    //sirenAudioSource.PlayDelayed(25.0f);
                    //sirenAudioSource.SetScheduledEndTime(AudioSettings.dspTime + 45.0f);
                    //ScheduleAudioClip(portal);
                    PublishNotification("Use left joystick to drive around");
                    break;
                case TrainingStep.DONE:
                    ScheduleAudioClip(miscAudio.ready, delay: 3);
                    break;
                default: break;
            }

        }

        public bool IsAudioPlaying()
        {
            bool playing = false;
            foreach (var source in audioSourceArray)
            {
                playing = playing || source.isPlaying;
            }
            return playing;
        }


        void Update()
        {
            if (startTraining && !IsAudioPlaying() && currentStep == TrainingStep.IDLE)
            {
                currentStep = TrainingStep.IDLE;
                Debug.Log("Started Training");
                NextStep();
                startTraining = false;
                //trainingStarted = true;
            }
            if (currentStep == TrainingStep.DONE && !IsAudioPlaying())
                StateManager.Instance.GoToState(StateManager.States.HUD);
            //if (currentStep == TrainingStep.HEAD && !isAudioPlaying())
            //    waitingForNod = true;

            // allows to continue to the next step when pressing 'n'
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextStep();
            }
        }
    }


}
