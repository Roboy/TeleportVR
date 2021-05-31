using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG.Calibration;
using Widgets;

namespace Training.Calibration
{
    public class HandTestStep : MonoBehaviour
    {
        public HandCalibrator leftCalibrator, rightCalibrator;
        public float maxError = 0.01f;
        public Completion completionWidget;

        public float dwellTime = 2;
        private Timer dwellTimer = new Timer();

        private PoseBuffer leftBuffer = new PoseBuffer(2), rightBuffer = new PoseBuffer(2);
        private bool refPoseAdded = false;

        // Start is called before the first frame update
        void Start()
        {
            // advance step, when dwell timer is done
            dwellTimer.SetTimer(dwellTime, () =>
            {
                Debug.Log("Dwell done");
                TutorialSteps.Instance.NextStep();
                completionWidget.active = false;
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (TutorialSteps.Instance.currentStep != TutorialSteps.TrainingStep.HAND_TEST)
            {
                return;
            }


            if (!TutorialSteps.Instance.IsAudioPlaying())
            {
                // refresh buffers
                leftBuffer.Clear();
                rightBuffer.Clear();
                leftBuffer.AddPose(leftCalibrator.poseValues[(int)HandCalibrator.Pose.ThumbUp]);
                rightBuffer.AddPose(rightCalibrator.poseValues[(int)HandCalibrator.Pose.ThumbUp]);
                leftBuffer.AddPose(leftCalibrator.GetCalibrationValues());
                rightBuffer.AddPose(rightCalibrator.GetCalibrationValues());

                dwellTimer.LetTimePass(Time.deltaTime);
                float leftError = leftBuffer.ComputeError(), rightError = rightBuffer.ComputeError();
                if (Mathf.Max(leftError, rightError) >= maxError)
                {
                    Debug.Log($"Error too large {leftError}, {rightError}, resetting dwell timer");
                    dwellTimer.ResetTimer();
                }

                completionWidget.active = true;
                completionWidget.progress = dwellTimer.GetFraction();
                Debug.Log($"Difference to calibrated thumbs up, left: {leftError} right: {rightError}");

            }
        }
    }

}
