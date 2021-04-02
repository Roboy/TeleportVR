using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : Singleton<EmotionManager>
{
    private RoboyAnimator faceAnimator;

    private static Dictionary<int, string> intToEmotion = new Dictionary<int, string>()
    {
        {0, "idle"},
        {1, "kiss"},
        {2, "img:money"},
        {3, "angry_new"},
        {4, "shy"},
        {5, "lookleft"},
        {6, "KeyCode"},
        {7, "blink"},
        {8, "tongue_out"},
        {9, "smileblink"},
        {10, "happy"},
        {11, "happy2"},
        {12, "hearts"},
        {13, "angry"},
        {14, "pissed"},
        {15, "hypno"},
        {16, "hypno_color"},
        {17, "rolling"},
        {18, "surprise_mit_augen"},
    };

    private static Dictionary<string, string> stringToEmotion = new Dictionary<string, string>()
    {
        {"A", "hearts"},
        {"B", "shy"},
        {"X", "blink"},
        {"Y", "img:money"},
    };

    /// <summary>
    /// Set the face by providing a button key combination
    /// </summary>
    /// <param name="key">The key name, e.g. 'X' for the X button.</param>
    public void SetFaceByKey(string key)
    {
        if (stringToEmotion.ContainsKey(key))
        {
            SetFace(stringToEmotion[key]);
        }
        else
        {
            print("Trying to set undefined Emotion: " + key);
        }
    }

    /// <summary>
    /// Set the Face to an emotion specified by a float. Currently not used, but will be used when feedback from animus
    /// is used to the the face.
    /// </summary>
    /// <param name="emotion"></param>
    public void SetFace(float emotion)
    {
        SetFace((int) emotion);
    }

    /// <summary>
    /// Sets the Face to the given emotionId.
    /// </summary>
    /// <param name="emotionId">The emotion Id in range [0, intToEmotion.Count]</param>
    public void SetFace(int emotionId)
    {
        if (emotionId < 0 || emotionId >= intToEmotion.Count)
        {
            Debug.LogWarning("Emotion with id " + emotionId + " is not in the valid range 0 to " +
                             (intToEmotion.Count - 1));
            emotionId = 0;
        }

        SetFace(intToEmotion[emotionId]);
    }

    /// <summary>
    /// Set the Face to the given emotion string.
    /// </summary>
    /// <param name="emotion"></param>
    public void SetFace(string emotion)
    {
        if (faceAnimator == null)
        {
            faceAnimator = FindObjectOfType<RoboyAnimator>();
            if (faceAnimator == null)
            {
                Debug.LogWarning("FaceAnimator could not be found.");
                return;
            }
        }

        faceAnimator.SetEmotion(emotion);
        print("Set emotion " + emotion);
    }
}