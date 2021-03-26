using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : Singleton<EmotionManager>
{
    private RoboyAnimator faceAnimator;

    private static Dictionary<int, string> intToEmotion = new Dictionary<int, string>() {
        {0,     "idle"},
        {1,     "kiss"},
        {2,     "img:money"},
        {3,     "angry_new"},
        {4,     "shy"},
        {5,     "lookleft"},
        {6,     "KeyCode"},
        {7,     "blink"},
        {8,     "tongue_out"},
        {9,     "smileblink"},
        {10,    "happy"},
        {11,    "happy2"},
        {12,    "hearts"},
        {13,    "angry"},
        {14,    "pissed"},
        {15,    "hypno"},
        {16,    "hypno_color"},
        {17,    "rolling"},
        {18,    "surprise_mit_augen"},
    };
    
    private static Dictionary<string, string> stringToEmotion = new Dictionary<string, string>() {
        {"A",     "hearts"},
        {"B",     "shy"},
        {"X",     "blink"},
        {"Y",     "img:money"},
    };

    void Update()
    {
        if (true)//StateManager.Instance.currentState == StateManager.States.HUD)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Input.GetKeyDown("" + i))
                {
                    if (i == 0)
                    {
                        UnityAnimusClient.Instance.LeftButton1 = true;
                    }
                    if (i == 1)
                    {
                        UnityAnimusClient.Instance.LeftButton2 = true;
                    }
                    if (i == 2)
                    {
                        UnityAnimusClient.Instance.RightButton1 = true;
                    }
                    if (i == 3)
                    {
                        UnityAnimusClient.Instance.RightButton2 = true;
                    }
                    //SetFace(i);
                }
                
                if (Input.GetKeyUp("" + i))
                {
                    if (i == 0)
                    {
                        UnityAnimusClient.Instance.LeftButton1 = false;
                    }
                    if (i == 1)
                    {
                        UnityAnimusClient.Instance.LeftButton2 = false;
                    }
                    if (i == 2)
                    {
                        UnityAnimusClient.Instance.RightButton1 = false;
                    }
                    if (i == 3)
                    {
                        UnityAnimusClient.Instance.RightButton2 = false;
                    }
                    //SetFace(i);
                }
            }
        }
    }

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

    public void SetFace(float emotion)
    {
        SetFace((int)emotion);
    }

    public void SetFace(int emotionId)
    {
        if (emotionId < 0 || emotionId >= intToEmotion.Count)
        {
            Debug.LogWarning("Emotion with id " + emotionId + " is not in the valid range 0 to " + (intToEmotion.Count - 1));
            emotionId = 0;
        }
        SetFace(intToEmotion[emotionId]);
    }

    public void SetFace(string emotion)
    {
        if (faceAnimator == null)
        {
            faceAnimator = GameObject.FindObjectOfType<RoboyAnimator>();
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
