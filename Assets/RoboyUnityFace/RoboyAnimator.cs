using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class RoboyAnimator : MonoBehaviour
{
    public Vector3 targetHeadEulerAngle;
    public List<Sprite> icons;
    public Image emojiRight;
    public Image emojiLeft;
    Animator anim;
    bool pirate = false;
    bool cryingRoboy = false;
    bool specs = false;
    bool sunglasses_on = false;
    bool moustache = false;
    bool talking = false;
    List<string> emotions = new List<string>();

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetBool("talking", talking);
        //anim.SetBool("cry", cryingRoboy);
        //anim.SetBool("moustache", moustache);
        //anim.SetBool("pirate", pirate);
        //anim.SetBool("sunglasses_on", sunglasses_on);
       // anim.SetBool("specs", specs);

        foreach (string e in emotions)
        {
            SetEmotion(e);
        }
        emotions.Clear();

        //if (UnityEngine.Random.value < 0.001f)
        //    anim.SetTrigger("blink");

        //KeyboardControls();

    }

    //void KeyboardControls()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        SetEmotion("url:https:upload.wikimedia.org/wikipedia/commons/7/7e/Cute-Ball-Favorites-icon.png");
    //    }
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        SetEmotion("img:money");
    //    }

    //    if (Input.GetKeyDown(KeyCode.A))
    //        SetEmotion("angry_new");
    //    if (Input.GetKeyDown(KeyCode.S))
    //        SetEmotion("shy");
    //    if (Input.GetKeyDown(KeyCode.K))
    //        SetEmotion("kiss");
    //    if (Input.GetKeyDown(KeyCode.L))
    //        SetEmotion("lookleft");
    //    if (Input.GetKeyDown(KeyCode.R))
    //        SetEmotion("lookright");
    //    if (Input.GetKeyDown(KeyCode.B))
    //        SetEmotion("blink");
    //    if (Input.GetKeyDown(KeyCode.D))
    //        SetEmotion("tongue_out");
    //    if (Input.GetKeyDown(KeyCode.W))
    //        SetEmotion("smileblink");
    //    if (Input.GetKeyDown(KeyCode.Q))
    //        SetEmotion("happy");
    //    if (Input.GetKeyDown(KeyCode.Y))
    //        SetEmotion("happy2");
    //    if (Input.GetKeyDown(KeyCode.H))
    //        SetEmotion("hearts");
    //    if (Input.GetKeyDown(KeyCode.N))
    //        SetEmotion("angry");
    //    if (Input.GetKeyDown(KeyCode.X))
    //        SetEmotion("pissed");
    //    if (Input.GetKeyDown(KeyCode.V))
    //        SetEmotion("hypno");
    //    if (Input.GetKeyDown(KeyCode.U))
    //        SetEmotion("hypno_color");
    //    if (Input.GetKeyDown(KeyCode.I))
    //        SetEmotion("rolling");
    //    if (Input.GetKeyDown(KeyCode.Z))
    //        SetEmotion("surprise_mit_augen");
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        pirate = !pirate;
    //    }

    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        cryingRoboy = !cryingRoboy;
    //    }

    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        specs = !specs;
    //    }


    //    if (Input.GetKeyDown(KeyCode.M))
    //    {
    //        moustache = !moustache;
    //    }

    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        sunglasses_on = !sunglasses_on;
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space))
    //        talking = true;
    //    if (Input.GetKeyUp(KeyCode.Space))
    //        talking = false;
    //}

    public void SetEmotion(string emotion)
    {
        switch (emotion)
        {
            case "hearts":
                emotion = "img:Heart";
                break;
            case "cry":
                cryingRoboy = !cryingRoboy;
                break;
            case "moustache":
                moustache = !moustache;
                break;
            case "pirate":
                pirate = !pirate;
                break;
            case "sunglasses_on":
                sunglasses_on = !sunglasses_on;
                break;
            case "talking":
                talking = !talking;
                break;
            default:
                break;
        }

        if (emotion.Contains("img:"))
        {
            string emoji = emotion.Substring(4);
            foreach (Sprite s in icons)
            {
                if (s.name == emoji)
                {
                    print(emojiLeft);
                    emojiLeft.sprite = s;
                    emojiRight.sprite = s;

                    anim.SetTrigger("hearts");
                    break;
                }
            }
        }
        else if (emotion.Contains("url:"))
        {
            string emoji = emotion.Substring(4);
            emojiLeft.sprite = IMG2Sprite.LoadNewSprite(emoji);
            emojiRight.sprite = IMG2Sprite.LoadNewSprite(emoji);
            anim.SetTrigger("hearts");
        }
        else
            anim.SetTrigger(emotion);
    }

    public void SetTalking(bool flag)
    {
        talking = flag;
    }
}