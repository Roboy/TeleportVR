using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TransitionManager : MonoBehaviour {
    PostProcessVolume postProcessVolume;

    [SerializeField] private float overallDuration = 1.5f;
    [SerializeField] private bool isLerping = false;

    private int curDirection = 1;

    private float lerpingValue;
    private float startValue = 0.0f, endValue = 1.0f;
    private float curTime = 0;

    void Start() {
        this.postProcessVolume = GetComponent<PostProcessVolume>();
        this.cameraOrigin = GameObject.FindGameObjectWithTag("CameraOrigin");
        this.walker = GameObject.FindGameObjectWithTag("KatVRWalker");
    }

    /// <summary>
    /// Listens on [isLerping] to perform visual transition.
    /// </summary>
    void Update() {
        if (isLerping) {
            curTime += Time.deltaTime;

            float flow = curTime / overallDuration;

            if (flow < 1) {
                lerpingValue = startValue + (endValue - startValue) * flow;
                this.postProcessVolume.weight = lerpingValue;
            }
            else {
                lerpingValue = startValue;
                float tmp = this.startValue;
                this.startValue = this.endValue;
                this.endValue = tmp;
                this.curTime = 0;

                if (curDirection == 1) {
                    curDirection = -1;
                }
                else if (curDirection == -1) {
                    curDirection = 1;
                    isLerping = false;
                }
            }
        }

        if (slerpStart != Vector3.zero || slerpStop != Vector3.zero) {
            cameraOrigin.transform.position = Vector3.Lerp(slerpStart, slerpStop, slerpTimer.GetFraction());
            slerpTimer.LetTimePass(Time.deltaTime);
        }
    }


    Timer slerpTimer = new Timer();
    GameObject cameraOrigin;
    GameObject walker;
    Vector3 slerpStart;
    Vector3 slerpStop;
    bool currentSceneHUD;

    /// <summary>
    /// Starts visual transition animation between scenes.
    /// </summary>
    public void StartTransition(bool hud) {
        this.isLerping = true;
        GetComponent<VestPlayer>().playTact();
        GetComponent<AudioSource>().Play();

        slerpTimer.SetTimer(2.0f, ResetTimer);
        slerpStart = cameraOrigin.transform.position;
        slerpStop = cameraOrigin.transform.position + cameraOrigin.transform.forward * (hud ? 1.5f : -1.5f); //KatVR not working: walker.transform.forward * (hud ? 1.5f : -1.5f);
        currentSceneHUD = hud;
    }

    public void ResetTimer() {
        if (currentSceneHUD) {
            Destroy(GameObject.FindGameObjectWithTag("Roboy"));
        }
        else {
            AnchorTransform[] anchorTransforms = GameObject.FindObjectsOfType<AnchorTransform>();
            for (int i = 0; i < anchorTransforms.Length; i++) {
                anchorTransforms[i].ResetAnchor();
            }
        }

        slerpStart = Vector3.zero;
        slerpStop = Vector3.zero;
        slerpTimer.ResetTimer();
    }
}