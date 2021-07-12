using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JointTransfer
{
    public class AngleRangeStore : MonoBehaviour
    {
        public Dictionary<string, Vector3> minRotation
        {
            get { return stored ? _minRotation : new Dictionary<string, Vector3>(); }
        }
        public Dictionary<string, Vector3> maxRotation
        {
            get { return stored ? _maxRotation : new Dictionary<string, Vector3>(); }
        }
        public bool stored = false;
        public string handCalibratorTag;
        public bool SaveOnQuit = true;

        private Dictionary<string, Transform> transforms;
        private Dictionary<string, Vector3> _minRotation;
        private Dictionary<string, Vector3> _maxRotation;
        private Training.Calibration.HandCalibrator calibrator;
        private const string FQN = "JointTransfer.AngleRangeStore";

        // Start is called before the first frame update
        void Start()
        {
            _minRotation = new Dictionary<string, Vector3>();
            _maxRotation = new Dictionary<string, Vector3>();
            transforms = new Dictionary<string, Transform>();
            foreach (var child in Utils.GetChildren(transform, obj => obj.name.Contains("_")))
            {
                string name = child.name;
                transforms[child.name] = child.transform;
            }

            if (SaveOnQuit)
            {
                stored = Load();
            }

            StartCoroutine(FindHandCalibrator());
        }

        // Update is called once per frame
        void Update()
        {
            if (calibrator != null && calibrator.calibrating && !stored)
            {
                foreach (var entry in transforms)
                {
                    string name = entry.Key;
                    var remapedAngles = Utils.RemapRotationVector3(entry.Value.localEulerAngles);
                    _minRotation[name] = Vector3.Min(
                        _minRotation.GetValueOrDefault(name, remapedAngles),
                        remapedAngles);
                    _maxRotation[name] = Vector3.Max(
                        _maxRotation.GetValueOrDefault(name, remapedAngles),
                        remapedAngles);
                }
            }
        }

        private IEnumerator FindHandCalibrator()
        {
            while (calibrator == null)
            {
                var obj = GameObject.FindGameObjectWithTag(handCalibratorTag);
                if (obj != null)
                {
                    calibrator = obj.GetComponent<Training.Calibration.HandCalibrator>();
                    calibrator.OnDone((step) =>
                    {
                        stored = true;
                        Save();
                    });
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        void OnDestroy()
        {
            Save();
        }

        private void Save()
        {
            foreach (var entry in minRotation)
            {
                PlayerPrefs.SetFloat($"{FQN}_minRotation_{handCalibratorTag}_{entry.Key}_X", entry.Value.x);
                PlayerPrefs.SetFloat($"{FQN}_minRotation_{handCalibratorTag}_{entry.Key}_Y", entry.Value.y);
                PlayerPrefs.SetFloat($"{FQN}_minRotation_{handCalibratorTag}_{entry.Key}_Z", entry.Value.z);
            }
            foreach (var entry in maxRotation)
            {
                PlayerPrefs.SetFloat($"{FQN}_maxRotation_{handCalibratorTag}_{entry.Key}_X", entry.Value.x);
                PlayerPrefs.SetFloat($"{FQN}_maxRotation_{handCalibratorTag}_{entry.Key}_Y", entry.Value.y);
                PlayerPrefs.SetFloat($"{FQN}_maxRotation_{handCalibratorTag}_{entry.Key}_Z", entry.Value.z);
            }
            PlayerPrefs.Save();
            Debug.Log($"Saved AngleRanges for {handCalibratorTag} in PlayerPrefs");
        }

        private bool Load()
        {
            Dictionary<string, Vector3> min = new Dictionary<string, Vector3>();
            Dictionary<string, Vector3> max = new Dictionary<string, Vector3>();

            foreach (var entry in transforms)
            {
                string nameX = $"{FQN}_minRotation_{handCalibratorTag}_{entry.Key}_X";
                string nameY = $"{FQN}_minRotation_{handCalibratorTag}_{entry.Key}_Y";
                string nameZ = $"{FQN}_minRotation_{handCalibratorTag}_{entry.Key}_Z";
                if (!PlayerPrefs.HasKey(nameX) || !PlayerPrefs.HasKey(nameY) || !PlayerPrefs.HasKey(nameZ))
                {
                    Debug.LogWarning($"Could not load AngleRanges for {handCalibratorTag} in PlayerPrefs");
                    return false;
                }
                min[entry.Key] = new Vector3(PlayerPrefs.GetFloat(nameX), PlayerPrefs.GetFloat(nameY), PlayerPrefs.GetFloat(nameZ));

                nameX = $"{FQN}_maxRotation_{handCalibratorTag}_{entry.Key}_X";
                nameY = $"{FQN}_maxRotation_{handCalibratorTag}_{entry.Key}_Y";
                nameZ = $"{FQN}_maxRotation_{handCalibratorTag}_{entry.Key}_Z";
                if (!PlayerPrefs.HasKey(nameX) || !PlayerPrefs.HasKey(nameY) || !PlayerPrefs.HasKey(nameZ))
                {
                    Debug.LogWarning($"Could not load AngleRanges for {handCalibratorTag} in PlayerPrefs");
                    return false;
                }
                max[entry.Key] = new Vector3(PlayerPrefs.GetFloat(nameX), PlayerPrefs.GetFloat(nameY), PlayerPrefs.GetFloat(nameZ));
            }
            Debug.Log($"Loaded AngleRanges for {handCalibratorTag} in PlayerPrefs");
            _minRotation = min;
            _maxRotation = max;
            return true;
        }
    }
}
