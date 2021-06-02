using UnityEngine;

namespace Training.AudioClips
{
    [System.Serializable]
    public struct Controller
    {
        public AudioClip leftArm, leftBall,
            rightArm, rightBall, leftHand, rightHand;
    }
    [System.Serializable]
    public struct SGTraining
    {
        public AudioClip leftArm, leftBall,
            leftHandStart, rightHandStart,
            rightArm, rightBall;
    }

    [System.Serializable]
    public struct SGHand
    {
        public AudioClip handOpen, handClosed, fingersExt, fingersFlexed,
            thumbUp, thumbFlex, abdOut, noThumbAbd, test;
    }

    [System.Serializable]
    public struct DriveJoystick
    {
        public AudioClip drive;
    }

    [System.Serializable]
    public struct Misc
    {
        public AudioClip welcome, imAria, head, nod, wrongTrigger, portal, enterButton,
            emergency, wrongGrip, wrongButton, siren, ready;
    }

}
