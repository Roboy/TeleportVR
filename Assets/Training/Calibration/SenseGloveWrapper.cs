using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SG;
using SenseGloveCs.Kinematics;
using SG.Calibration;

namespace Training.Calibration
{
    public class SenseGloveWrapper: MonoBehaviour
    {
        SG_SenseGloveHardware glove;
        InterpolationSet_IMU interpolator;
        CalibrationPose[] poses;
        bool isRight; 
        string handName
        {
            get { return isRight ? "right" : "left"; }
        }
        
        public enum Pose
        {
            HandOpen = 0,
            HandClosed = 1,
            FingersExt = 2,
            FingersFlexed = 3,
            ThumbUp = 4,
            ThumbFlex = 5,
            AbdOut = 6,
            NoThumbAbd = 7,
        }


        void Start()
        {
            foreach (SG.SG_SenseGloveHardware obj in GameObject.FindObjectsOfType(typeof(SG.SG_SenseGloveHardware)))
            {
                string connectionMethod = obj.connectionMethod.ToString().ToLower();
                bool match = isRight ? connectionMethod.Equals("nextrighthand") : connectionMethod.Equals("nextlefthand");
                if (match)
                {
                    glove= obj;
                }
            }

            if (glove == null)
            {
                throw new MissingComponentException($"Could not find {handName} SG_SenseGloveHardware in Scene.");
            }
            
            Debug.Log($"Awaiting connection with {(isRight ? "right" : "left")} SenseGlove... ");
        }

        void Update()
        {
            if (interpolator == null &&
                glove.IsLinked &&
                glove.GetInterpolationProfile(out interpolator))
            {
                poses = LoadProfiles(interpolator);
                Debug.Log($"Connected {handName} glove");
            }
        }

        private CalibrationPose[] LoadProfiles(InterpolationSet_IMU interpolator)
        {
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

    }

}
