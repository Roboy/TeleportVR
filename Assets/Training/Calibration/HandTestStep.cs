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
        public float maxError = 0.5f;
        public Completion completionWidget;

        public float dwellTime = 3;
        private Timer dwellTimer = new Timer();

        private PoseBuffer leftBuffer = new PoseBuffer(2);
        private PoseBuffer rightBuffer = new PoseBuffer(2);

        private bool initCompletion = false;

        // Start is called before the first frame update
        void Start()
        {

            // advance step, when dwell timer is done
            dwellTimer.SetTimer(dwellTime, () =>
            {
                Debug.Log("Dwell done");
                completionWidget.active = false;
                TutorialSteps.Instance.NextStep();
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (TutorialSteps.Instance.currentStep != TutorialSteps.TrainingStep.HAND_TEST)
            {
                return;
            }


            if (TutorialSteps.Instance.IsAudioPlaying())
            {
                return;
            }

            if (!initCompletion)
            {
                completionWidget.text = "Hold";
                completionWidget.active = true;
                initCompletion = true;
            }

            // refresh buffers
            leftBuffer.Clear();
            rightBuffer.Clear();
            leftBuffer.AddPose(leftCalibrator.poseValues[(int)HandCalibrator.Pose.ThumbUp]);
            rightBuffer.AddPose(rightCalibrator.poseValues[(int)HandCalibrator.Pose.ThumbUp]);
            leftBuffer.AddPose(leftCalibrator.GetCalibrationValues());
            rightBuffer.AddPose(rightCalibrator.GetCalibrationValues());

            float error = Mathf.Max(leftBuffer.ComputeError(), rightBuffer.ComputeError());
            Debug.Log($"Error: {error}");
            if (error > maxError)
            {
                dwellTimer.ResetTimer();
                return;
            }

            dwellTimer.LetTimePass(Time.deltaTime);
            completionWidget.progress = dwellTimer.GetFraction();
        }
    }

}
