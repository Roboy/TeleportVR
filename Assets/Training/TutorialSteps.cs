using System.Collections.Generic;
using UnityEngine;
using Widgets;

namespace Training
{
    public class TutorialSteps : MonoBehaviour//Singleton<TutorialSteps>
    {
        public static TutorialSteps Instance;
        
        public int currentStep;
        public AudioClip welcome, imAria,headHowTo, leftArmHowTo, rightArmHowTo, driveHowTo, amazing;
        public AudioSource[] audioSourceArray;
        int toggle;
        double prevDuration = 0.0;
        private AudioSource _audioSource;

        [SerializeField] private GameObject designatedArea;
        //[SerializeField] private GameObject designatedArea;
    
        // Start is called before the first frame update
        void Start()
        {
            // get a reference to this singleton, as scripts from other scenes are not able to do this
            _ = Instance;
            print(Instance);
            
            _audioSource = GetComponent<AudioSource>();

            //ScheduleAudioClip(welcome);
            //ScheduleAudioClip(imAria, 2.0);
           
            PublishNotification("Welcome to the Training!"); //\n" +
                                //"Take a look around. " +
                                //"In the mirror you can see how you are controlling the Head of Roboy.\n" +
                                //"Look at the blue sphere to get started!");
        }

        private void ScheduleAudioClip(AudioClip clip, double delay=0)
        {
            toggle = 1 - toggle;
            audioSourceArray[toggle].clip = clip;
            audioSourceArray[toggle].PlayScheduled(AudioSettings.dspTime + prevDuration + delay);
            prevDuration = (double)clip.samples / clip.frequency;

        }



        public static void PublishNotification(string message)
        {
            Widget notificationWidget = Manager.Instance.FindWidgetWithID(10);
            RosJsonMessage toastrMessage = RosJsonMessage.CreateToastrMessage(10, message, 5,
                new byte[] { 255, 40, 15, 255 });
            notificationWidget.ProcessRosMessage(toastrMessage);
        }

        public void NextStep()
        {
            currentStep++;
            if (currentStep == 1)
            {
                prevDuration = 0;
                ScheduleAudioClip(headHowTo);
                PublishNotification("head");
                //PublishNotification("You can move Roboy's wheelchair by using your left Joystick.");
                //_audioSource.Play();
                
            }
            else if (currentStep == 2)
            {
                PublishNotification("Let's get a bit closer to the sphere.");
            }
            else if (currentStep == 3)
            {
                PublishNotification("To move your arm, hold down the hand trigger on the controller with your middle finger.");
            }
            else if (currentStep == 4)
            {
                PublishNotification("Let's touch the sphere with your hand.");
            }
            else if (currentStep == 5)
            {
                PublishNotification("Now press the trigger down with your index finger to grab the sphere.");
            }
            else if (currentStep == 6)
            {
                PublishNotification("Well done, young Roboyan. You are now ready to control Roboy.");
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextStep();
            }
        }
    }
}
