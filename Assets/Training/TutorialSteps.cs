using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Widgets;

namespace Training
{
    public class TutorialSteps : MonoBehaviour//Singleton<TutorialSteps>
    {
        public static TutorialSteps Instance;

        public TrainingStep currentStep;
        public AudioClip welcome, imAria, headHowTo, leftArmHowTo, leftBall, rightArmHowTo, rightBall, handHowTo, hand2HowTo, driveHowTo, nod, wrongTrigger, portal, enterbtn, emergency, wrongGrip, wrongButton, siren, ready;
        public List<AudioClip> praisePhrases = new List<AudioClip>();
        public AudioSource[] audioSourceArray;
        public AudioSource sirenAudioSource;
        public bool waitingForNod = false;
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
        //[SerializeField] private GameObject designatedArea;


        void Start()
        {
            Debug.Log(StateManager.Instance.TimesStateVisited(StateManager.States.Training));
            // get a reference to this singleton, as scripts from other scenes are not able to do this
            _ = Instance;
            if (StateManager.Instance.TimesStateVisited(StateManager.States.Training) <= 1)
            {
                ScheduleAudioClip(welcome, queue: true, delay: 1.0);
                ScheduleAudioClip(imAria, queue: true);//, delay: 2.0);

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
            if (isAudioPlaying() && queue)
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
        public static void PublishNotification(string message)
        {
            Widget notificationWidget = Manager.Instance.FindWidgetWithID(10);
            RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, message, 5,
                new byte[] { 255, 40, 15, 255 });
            notificationWidget.ProcessRosMessage(toastrMessage);
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
                    audio = wrongTrigger;
                    break;
                case "grip":
                    audio = wrongGrip;
                    break;
                default:
                    audio = wrongButton;
                    break;
            }
            Debug.Log("Correcting User");
            if (lastCorrectedAtStep != currentStep && (currentStep == TrainingStep.LEFT_ARM || currentStep == TrainingStep.RIGHT_ARM))
            {
                ScheduleAudioClip(wrongTrigger);
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
            Debug.Log("current step: " + currentStep);
            if (currentStep == TrainingStep.HEAD)
            {
                ScheduleAudioClip(headHowTo);
                PublishNotification("Try moving your head around");
                ScheduleAudioClip(nod, delay: 0);
                waitingForNod = true;

            }
            else if (currentStep == TrainingStep.LEFT_ARM)
            {
                ScheduleAudioClip(leftArmHowTo, queue: true);
                ScheduleAudioClip(leftBall, queue: true);
                PublishNotification("Press and hold the index trigger and try moving your left arm");
                var colTF = PlayerRig.Instance.transform.position;
                colTF.y -= 0.1f;
                colTF.z += 0.2f;
                handCollectables.transform.position = colTF;
                handCollectables.Find("HandCollectableLeft").gameObject.SetActive(true);
            }
            else if (currentStep == TrainingStep.LEFT_HAND)
            {
                ScheduleAudioClip(handHowTo, queue: true, delay: 0);
                PublishNotification("Press the grip button on the side to close the hand.");
            }
            else if (currentStep == TrainingStep.RIGHT_ARM)
            {
                ScheduleAudioClip(rightArmHowTo, delay: 0);
                ScheduleAudioClip(rightBall,queue: true);
                PublishNotification("Press and hold the index trigger and try moving your right arm");
                //PublishNotification("To move your arm, hold down the hand trigger on the controller with your middle finger.");
                handCollectables.Find("HandCollectableRight").gameObject.SetActive(true);
            }
            else if (currentStep == TrainingStep.RIGHT_HAND)
            {
                ScheduleAudioClip(hand2HowTo, queue: true, delay: 0);
                PublishNotification("Press the grip button to close the hand.");
            }

            else if (currentStep == TrainingStep.WHEELCHAIR)
            {
                ScheduleAudioClip(driveHowTo, delay: 1);
                //ScheduleAudioClip(emergency, queue: true);
                
                //sirenAudioSource.PlayDelayed(25.0f);
                //sirenAudioSource.SetScheduledEndTime(AudioSettings.dspTime + 45.0f);
                //ScheduleAudioClip(portal);
                PublishNotification("Use left joystick to drive around");
            }
            else if (currentStep == TrainingStep.DONE)
            {
                ScheduleAudioClip(ready, delay: 2);
            }
        }

        bool isAudioPlaying()
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
            if (startTraining && !isAudioPlaying())
            {
                currentStep = TrainingStep.IDLE;
                Debug.Log("moved to the next step");
                NextStep();
                startTraining = false;
                //trainingStarted = true;
            }
           // if (currentStep == TrainingStep.DONE && !isAudioPlaying())
            //    StateManager.Instance.GoToState(StateManager.States.HUD);
            
            //if (currentStep == TrainingStep.HEAD && !isAudioPlaying())
            //    waitingForNod = true;

            // allows to continue to the next step when pressing 'n'
            if (Keyboard.current[Key.N].wasPressedThisFrame ) //Input.GetKeyDown(KeyCode.N))
            {
                NextStep();
            }
        }
    }
}
