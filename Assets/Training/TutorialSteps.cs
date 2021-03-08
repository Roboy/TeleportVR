using UnityEngine;
using Widgets;

namespace Training
{
    public class TutorialSteps : Singleton<TutorialSteps>
    {
        public int currentStep;
        private AudioSource _audioSource;

        [SerializeField] private GameObject designatedArea;
        //[SerializeField] private GameObject designatedArea;
    
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            PublishNotification("Welcome to the Training!");
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
                PublishNotification("Well done!\nYou can move by using your left Joystick.");
                _audioSource.Play();
                
            }
            else if (currentStep == 2)
            {
                PublishNotification("Move the wheelchair to the designated area!");
            }
            else if (currentStep == 3)
            {
                PublishNotification("Move your arm to the sphere by holding down the hand trigger on the controller with your middle finger.");
            }
            else if (currentStep == 4)
            {
                PublishNotification("Now press the trigger down with your index finger to grab the sphere.");
            }
            else if (currentStep == 5)
            {
                PublishNotification("Well done.");
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
