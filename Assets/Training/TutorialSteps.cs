using System.Collections.Generic;
using UnityEngine;
using Widgets;

namespace Training
{
    public class TutorialSteps : MonoBehaviour//Singleton<TutorialSteps>
    {
        public static TutorialSteps Instance;
        
        public TrainingStep currentStep;
        public AudioClip welcome, imAria,headHowTo, leftArmHowTo, rightArmHowTo, handHowTo, hand2HowTo, driveHowTo, amazing, nod, wrongTrigger;
        public List<AudioClip> praisePhrases = new List<AudioClip>();
        public AudioSource[] audioSourceArray;
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
            WHEELCHAIR
        }
        
        [SerializeField] private Transform handCollectables;

        [SerializeField] private GameObject designatedArea;
        //[SerializeField] private GameObject designatedArea;
    
        // Start is called before the first frame update
        void Start()
        {
            
            Debug.Log(StateManager.Instance.TimesStateVisited(StateManager.States.Training));
            // get a reference to this singleton, as scripts from other scenes are not able to do this
            _ = Instance;
            if (StateManager.Instance.TimesStateVisited(StateManager.States.Training) <= 1)
            {
                //ScheduleAudioClip(welcome, delay: 1.0);
                //ScheduleAudioClip(imAria, delay: 2.0);

                PublishNotification("Welcome to the Training!"); //\n" +
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

            //trainingStarted = false;
        }

        private void ScheduleAudioClip(AudioClip clip, double delay=0)
        {
            var timeLeft = 0.0;
            
            if (isAudioPlaying())
            {
                timeLeft = prevDuration - (AudioSettings.dspTime - prevStart);
                if (timeLeft > 0) delay = timeLeft;
            }

            toggle = 1 - toggle;
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

        public void CorrectUser()
        {
            Debug.Log("Correcting User");
            if (lastCorrectedAtStep != currentStep &&  (currentStep == TrainingStep.LEFT_ARM || currentStep == TrainingStep.RIGHT_ARM))
            {
                ScheduleAudioClip(wrongTrigger);
                lastCorrectedAtStep = currentStep;
            }
        }

        public void NextStep(bool praise=false)
        {
            if (praise)
                PraiseUser();
            currentStep++;
            Debug.Log("current step: " + currentStep);
            if (currentStep == TrainingStep.HEAD)
            {
                ScheduleAudioClip(headHowTo);
                PublishNotification("Try moving your head around");
                ScheduleAudioClip(nod, delay: 0);
                waitingForNod = true;

                //PublishNotification("You can move Roboy's wheelchair by using your left Joystick.");
                //_audioSource.Play();

            }
            else if (currentStep == TrainingStep.LEFT_ARM)
            {
                ScheduleAudioClip(leftArmHowTo, delay: 1);
                PublishNotification("Press and hold the grip trigger and try moving your left arm");
                var colTF = PlayerRig.Instance.transform.position;
                colTF.y -= 0.1f;
                colTF.z += 0.2f;
                handCollectables.transform.position = colTF;
                handCollectables.Find("HandCollectableLeft").gameObject.SetActive(true);
            }
            else if (currentStep == TrainingStep.LEFT_HAND)
            {
                ScheduleAudioClip(handHowTo, delay: 0);
                PublishNotification("Press the trigger down with your index finger to close the hand.");
            }
            else if (currentStep == TrainingStep.RIGHT_ARM)
            {
                ScheduleAudioClip(rightArmHowTo, delay: 0);
                PublishNotification("Press and hold the grip trigger and try moving your right arm");
                //PublishNotification("To move your arm, hold down the hand trigger on the controller with your middle finger.");
                handCollectables.Find("HandCollectableRight").gameObject.SetActive(true);
                //handCollectables.gameObject.SetActive(true);
            }
            else if (currentStep == TrainingStep.RIGHT_HAND)
            {
                ScheduleAudioClip(hand2HowTo, delay: 0);
                PublishNotification("Press the trigger down with your index finger to close the hand.");
            }

            else if (currentStep == TrainingStep.WHEELCHAIR)
            {
                ScheduleAudioClip(driveHowTo, delay:1);
                PublishNotification("Use left joystick to drive around");
                //PublishNotification("Let's touch the sphere with your hand.");
            }
            
            //else if (currentStep == 6)
            //{
            //    PublishNotification("Well done, young Roboyan. You are now ready to control Roboy.");
            //}
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

        // Update is called once per frame
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
            //if (currentStep == TrainingStep.HEAD && !isAudioPlaying())
            //    waitingForNod = true;
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextStep();
            }
        }
    }
}
