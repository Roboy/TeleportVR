using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animus.Data;
using Animus.RobotProto;
using AnimusCommon;
using Google.Protobuf.Collections;
using BioIK;
#if ANIMUS_USE_OPENCV
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
#endif
using UnityEngine;
using UnityEngine.Networking;
using Widgets;
using Quaternion = UnityEngine.Quaternion;
using Transform = UnityEngine.Transform;
using Vector3 = UnityEngine.Vector3;

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        //Simply return true no matter what
        return true;
    }
}

public class UnityAnimusClient : Singleton<UnityAnimusClient>
{
    public GameObject OVRRig;
    public Transform TrackingSpace;
    public Robot chosenDetails;

    // vision variables
    public bool stereovision = false;
    public GameObject LeftEye;
    public GameObject RightEye;
    [SerializeField] private GameObject _leftPlane;
    [SerializeField] private GameObject _rightPlane;
    private Renderer _leftRenderer;
    private Renderer _rightRenderer;
    private Texture2D _leftTexture;
    private Texture2D _rightTexture;
    private bool visionEnabled;

    private bool triggerResChange;

    // private List<int> imageDims;
    private RepeatedField<uint> _imageDims;
#if ANIMUS_USE_OPENCV
    private Mat yuv;
    private Mat rgb;
#endif

    private bool initMats;

    // motor variables
    public Transform robotHead;
    public Transform robotBase;
    public Transform robotLeftHandObjective;
    public Transform robotRightHandObjective;
    private Vector3 robotLeftHandPositionROS;
    private Vector3 robotRightHandPositionROS;
    private Vector3 robotHeadPositionROS;
    private Quaternion robotLeftHandOrientationROS;
    private Quaternion robotRightHandOrientationROS;
    private Quaternion robotHeadOrientationROS;
    public Transform humanRightHand;
    public Transform humanLeftHand;
    public Transform humanHead;
    public Vector3 bodyToBaseOffset;
    public float ForwardDeadzone;
    public float SidewaysDeadzone;
    public float RotationDeadzone;
    private float humanRightHandOpen;
    private float humanLeftHandOpen;
    private Vector2 eyesPosition;
    private bool trackingRight;
    private bool trackingLeft;

    // public NaoAnimusDriver robotDriver;
    public BioIK.BioIK _myIKBody;
    public BioIK.BioIK _myIKHead;
    private List<BioSegment> _actuatedJoints;
    private bool motorEnabled;
    private float _lastUpdate;

    private bool bodyTransitionReady;
    private int bodyTransitionDuration = 1;

    private Animus.Data.Float32Array motorMsg;
    private Sample motorSample;
    
    public static bool sendTransformToCam = false;
    public Transform wheelchair;
    
    // audition variables
    private bool auditionEnabled;
    // public GameObject Audio;
    // private AudioSetter _audioSetter;

    // voice variables
    // public GameObject Voice;
    private bool voiceEnabled;
    // private VoiceSampler _voiceSampler;

    // emotion variables
    public bool LeftButton1;
    public bool LeftButton2;
    public bool RightButton1;
    public bool RightButton2;
    public string currentEmotion;
    public string oldEmotion;
    private Animus.Data.StringSample emotionMsg;
    private Sample emotionSample;

    private const string LEDS_OFF = "off";
    private const string LEDS_CONNECTING = "robot_connecting";
    private const string LEDS_CONNECTED = "robot_established";
    private const string LEDS_IS_CONNECTED = "if_connected";

    public void Start()
    {
        motorEnabled = false;
        visionEnabled = false;
        auditionEnabled = false;
        voiceEnabled = false;
        initMats = false;
        bodyTransitionReady = false;

        var camRight = RightEye.transform.GetComponentInParent<Camera>();
        var camLeft = LeftEye.transform.GetComponentInParent<Camera>();
        if (!stereovision)
        {
            camRight.stereoTargetEye = StereoTargetEyeMask.None;

            camLeft.stereoTargetEye = StereoTargetEyeMask.Both;
        }
        else
        {
            camRight.stereoTargetEye = StereoTargetEyeMask.Right;

            camLeft.stereoTargetEye = StereoTargetEyeMask.Left;
        }

        // controls an led ring (optional)
        StartCoroutine(SendLEDCommand(LEDS_CONNECTING));
        StartCoroutine(StartBodyTransition());
    }


    IEnumerator StartBodyTransition()
    {
        yield return null;
        //robotBody.transform.eulerAngles = new Vector3(0, -180, 0);
        //
        yield return null;
        TrackingSpace = OVRRig.transform.Find("TrackingSpace");
        // humanHead = TrackingSpace.Find("CenterEyeAnchor");
        humanLeftHand = TrackingSpace.Find("LeftHandAnchor");
        humanRightHand = TrackingSpace.Find("RightHandAnchor");
        
        // The code below might be needed if the Body Transition gets implemented again
        // robotDriver = robotBody.GetComponent<NaoAnimusDriver>();
        // if (robotDriver != null)
        // {
        // 	robotBase = robotDriver.topCamera.gameObject.transform.parent.transform;
        // 	robotLeftHandObjective = robotDriver.leftHandTarget.transform;
        // 	robotRightHandObjective = robotDriver.rightHandTarget.transform;
        // 	bodyToBaseOffset = robotBase.position - robotBody.transform.position;
        // }
        // else
        // {
        // 	robotBase = robotBody.transform;
        // 	bodyToBaseOffset = Vector3.zero;
        // }
        //
        // var roboTransform = robotBody.transform;
        // Vector3 startPos = roboTransform.position;
        // Vector3 endPos = humanHead.position - bodyToBaseOffset;
        // Vector3 startAngles = roboTransform.eulerAngles;

        // 		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / bodyTransitionDuration)
        // 		{
        // 			bodyToBaseOffset = robotBase.position - robotBody.transform.position;
        // 			endPos = humanHead.position - bodyToBaseOffset;
        // 			roboTransform.position = new Vector3(Mathf.SmoothStep(startPos.x, endPos.x, t),
        // 												 Mathf.SmoothStep(startPos.y, endPos.y, t),
        // 												 Mathf.SmoothStep(startPos.z, endPos.z, t));

        // 			roboTransform.eulerAngles = new Vector3(Mathf.SmoothStep(startAngles.x, 0, t),
        // 													Mathf.SmoothStep(startAngles.y, 0, t),
        // 													Mathf.SmoothStep(startAngles.z, 0, t));
        // 			yield return null;
        // 		}

        bodyTransitionReady = true;
    }

    IEnumerator SendLEDCommand(string command)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://lib.roboy.org/teleportal/" + command))
        {
            webRequest.certificateHandler = new BypassCertificate();
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
        }
    }

    
	// --------------------------Spatial Modality----------------------------------
    	public bool spatial_initialise()
    	{
    		return true;
    	}
    
    	public bool spatial_set(Animus.Data.BlobSample currSample)
        {
	        int[] header = new int[4];
	        header[0] = (int)currSample.IntArray[0];
	        header[1] = (int)currSample.IntArray[1];
	        header[2] = (int)currSample.IntArray[2];
	        header[3] = (int)currSample.IntArray[3];
	        Debug.Log("Got packet " + currSample.IntArray[3]);

            if (InfiniTAMSender.instance != null)
            {
                InfiniTAMSender.instance.SendData(header, currSample.BytesArray[0].ToByteArray());    
            }

            return true;
    	}
    
    	public bool spatial_close()
    	{
    		return true;
    	}
	
	// --------------------------SpatialSlam Modality----------------------------------
	public bool spatialSlam_initialise()
	{
		return true;
	}

	public bool spatialSlam_set(Float32Array currSample)
	{
		Matrix4x4 cameraPos;
		
		#region MatrixCopy
		cameraPos.m00 = currSample.Data[0];
		cameraPos.m01 = currSample.Data[1];
		cameraPos.m02 = currSample.Data[2];
		cameraPos.m03 = currSample.Data[3];
		cameraPos.m10 = currSample.Data[4];
		cameraPos.m11 = currSample.Data[5];
		cameraPos.m12 = currSample.Data[6];
		cameraPos.m13 = currSample.Data[7];
		cameraPos.m20 = currSample.Data[8];
		cameraPos.m21 = currSample.Data[9];
		cameraPos.m22 = currSample.Data[10];
		cameraPos.m23 = currSample.Data[11];
		cameraPos.m30 = currSample.Data[12];
		cameraPos.m31 = currSample.Data[13];
		cameraPos.m32 = currSample.Data[14];
		cameraPos.m33 = currSample.Data[15];
		#endregion
        
		cameraPos = cameraPos.transpose;
		
		// TRANSLATION: invert y 
		Vector3 posTemp = new Vector3(cameraPos.m03, -cameraPos.m13, cameraPos.m23);
		// ROTATION: invert x and z axis
		Quaternion rotTemp = new Quaternion(-cameraPos.rotation.x, cameraPos.rotation.y, -cameraPos.rotation.z, cameraPos.rotation.w);

		if (sendTransformToCam)
		{
			Camera.main.transform.rotation = rotTemp;
			Camera.main.transform.position = posTemp;
		}

        wheelchair.transform.position = new Vector3(posTemp.x, wheelchair.transform.position.y, posTemp.z);
        Vector3 eulerAngles = wheelchair.transform.eulerAngles;
        eulerAngles.x = rotTemp.eulerAngles.y;
        wheelchair.transform.eulerAngles = eulerAngles; 
        
		//Debug.Log(currSample.Data[0] + ", " + currSample.Data[1] + ", " + currSample.Data[2]);
		return true;
	}

	public bool spatialSlam_close()
	{
		return false;
	}
    
    // --------------------------Vision Modality----------------------------------
    /// <summary>
    /// This method allows to display the current latency and fps in the connection widget. 
    /// </summary>
    /// <param name="latency">The current latency in ms.</param>
    /// <param name="fps">The frames per second.</param>
    public static void DisplayLatency(float latency, float fps)
    {
        // Present the latency and fps
        Widget latencyWidget = Manager.Instance.FindWidgetWithID(33);
        if (latency < 0 || latency > 100000)
        {
            latencyWidget.GetContext().textMessage = $"FPS: {fps:F2}";
        }
        else
        {
            latencyWidget.GetContext().textMessage = $"Latency: {latency:F2}ms\nFPS: {fps:F2}";
        }

        latencyWidget.ProcessRosMessage(latencyWidget.GetContext());

        // turn the icon from yellow (no connection) to green
        Widget wifiWidget = Manager.Instance.FindWidgetWithID(23);
        wifiWidget.GetContext().currentIcon = "WifiGreen";
        wifiWidget.ProcessRosMessage(wifiWidget.GetContext());
    }

    /// <summary>
    /// Setup the displays to display the received image(s) from animus.
    /// </summary>
    /// <returns>The success of this method.</returns>
    public bool vision_initialise()
    {
        //Get OVR Cameras
        var cameras = OVRRig.GetComponentsInChildren<Camera>();

        // Setup ovr camera parameters and attach component transforms to ovr camera transforms
        // This allows the planes to follow the cameras
        foreach (Camera cam in cameras)
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.orthographic = true;
            cam.orthographicSize = 5;
            cam.cullingMask &= ~(1 << 11);
            switch (cam.transform.name)
            {
                case "LeftEyeAnchor":
                    LeftEye.transform.parent = cam.transform;
                    LeftEye.transform.localPosition = Vector3.zero;
                    LeftEye.transform.localEulerAngles = Vector3.zero;
                    break;
                case "RightEyeAnchor":
                    RightEye.transform.parent = cam.transform;
                    RightEye.transform.localPosition = Vector3.zero;
                    RightEye.transform.localEulerAngles = Vector3.zero;
                    break;
            }
        }

        _leftRenderer = _leftPlane.GetComponent<Renderer>();
        _rightRenderer = _rightPlane.GetComponent<Renderer>();
        _imageDims = new RepeatedField<uint>();
        visionEnabled = true;

        return visionEnabled;
    }

    /// <summary>
    /// Returns the textures for both eyes, so that they can be used in other scripts, e.g. for the portal.
    /// </summary>
    /// <returns>Both eye textures.</returns>
    public Texture2D[] GetVisionTextures()
    {
        return new[] {_leftTexture, _rightTexture};
    }

    /// <summary>
    /// Activates or deactivates the displays, depending on the current scene and if stereovision is active
    /// </summary>
    private void SetDisplaystate()
    {
        if (AdditiveSceneManager.GetCurrentScene() == Scenes.HUD)
        {
            if (stereovision)
                _rightPlane.SetActive(true);
            _leftPlane.SetActive(true);
        }
        else
        {
            _rightPlane.SetActive(false);
            _leftPlane.SetActive(false);
        }
    }

    /// <summary>
    /// Display the received image(s) from animus.
    /// </summary>
    /// <param name="currSamples">The image samples received via animus.</param>
    /// <returns>Success of this method.</returns>
    public bool vision_set(ImageSamples currSamples)
    {
        try
        {
            if (!bodyTransitionReady) return true;

            if (!visionEnabled)
            {
                Debug.Log("Vision modality not enabled. Cannot set");
                return false;
            }

            if (currSamples == null)
            {
                return false;
            }

            var currSample = currSamples.Samples[0];
            var currShape = currSample.DataShape;

            var all_bytes = currSample.Data.ToByteArray();
#if ANIMUS_USE_OPENCV
            if (!initMats)
            {
                yuv = new Mat((int) (currShape[1] * 1.5), (int) currShape[0], CvType.CV_8UC1);
                rgb = new Mat();
                initMats = true;
            }

            if (all_bytes.Length != currShape[0] * currShape[1] * 1.5)
            {
                return true;
            }

            if (currShape[0] <= 100 || currShape[1] <= 100) // TODO delete the / 5
            {
                return true;
            }

            yuv.put(0, 0, all_bytes);

            Imgproc.cvtColor(yuv, rgb, Imgproc.COLOR_YUV2BGR_I420);
            if (_imageDims.Count == 0 || currShape[0] != _imageDims[0] || currShape[1] != _imageDims[1] ||
                currShape[2] != _imageDims[2])
            {
                _imageDims = currShape;
                var scaleX = (float) _imageDims[0] / (float) _imageDims[1];

                Debug.Log("Resize triggered. Setting texture resolution to " + currShape[0] + "x" + currShape[1]);
                Debug.Log("Setting horizontal scale to " + scaleX + " " + (float) _imageDims[0] + " " +
                          (float) _imageDims[1]);

                UnityEngine.Vector3 currentScale = _leftPlane.transform.localScale;
                currentScale.x = scaleX;
                currentScale.z /= scaleX;

                if (stereovision)
                {
                    _leftPlane.transform.localScale = currentScale;
                    // the left texture is the upper half of the received image
                    _leftTexture = new Texture2D(rgb.width(), rgb.height() / 2, TextureFormat.ARGB32, false)
                    {
                        wrapMode = TextureWrapMode.Clamp
                    };

                    _rightPlane.transform.localScale = currentScale;
                    // the right texture is the lower half of the received image
                    _rightTexture = new Texture2D(rgb.width(), rgb.height() / 2, TextureFormat.ARGB32, false)
                    {
                        wrapMode = TextureWrapMode.Clamp
                    };
                }
                else
                {
                    _leftPlane.transform.localScale = currentScale;
                    _leftTexture = new Texture2D(rgb.width(), rgb.height(), TextureFormat.ARGB32, false)
                    {
                        wrapMode = TextureWrapMode.Clamp
                    };
                }
            }

            if (stereovision)
            {
                // loop over the left and the right eye
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        // display the left image
                        Mat rgb_l = rgb.rowRange(0, rgb.rows() / 2);
                        Utils.matToTexture2D(rgb_l, _leftTexture);
                        _leftRenderer.material.mainTexture = _leftTexture;
                    }
                    else if (i == 1)
                    {
                        // display the right image
                        Mat rgb_r = rgb.rowRange(rgb.rows() / 2, rgb.rows());
                        Utils.matToTexture2D(rgb_r, _rightTexture);
                        _rightRenderer.material.mainTexture = _rightTexture;
                    }
                    else
                    {
                        print("Unknown image source: " + currSample.Source);
                    }
                }
            }
            else
            {
                Utils.matToTexture2D(rgb, _leftTexture);
                _leftRenderer.material.mainTexture = _leftTexture;
            }
#endif
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return true;
    }

    /// <summary>
    /// Closes the vision modality if it was opened and sets a corresponding flag.
    /// </summary>
    /// <returns>Closing the modality was successful.</returns>
    public bool vision_close()
    {
        if (!visionEnabled)
        {
            Debug.Log("Vision modality not enabled. Cannot close");
            return false;
        }

        visionEnabled = false;
        return true;
    }


    //--------------------------Audition Modality----------------------------------
    public bool audition_initialise()
    {
        return auditionEnabled;
    }

    public bool audition_set(AudioSample currSample)
    {
        if (!bodyTransitionReady) return true;
        return auditionEnabled;
    }

    public bool audition_close()
    {
        auditionEnabled = false;
        return true;
    }


    // --------------------------Collision Modality----------------------------------
    /// <summary>
    /// Nothing to do on startup, but the method will still be called by animus.
    /// </summary>
    /// <returns>Success.</returns>
    public bool collision_initialise()
    {
        return true;
    }

    /// <summary>
    /// Receive either Link Information (first float is 1) or Collision Info (first float is 2).
    /// </summary>
    /// <param name="collision">The float array containing the Link or Collision data.</param>
    /// <returns>Success.</returns>
    public bool collision_set(Float32Array collision)
    {
#if ROSSHARP
        // subtract the first float from the length of the actual data
        int collisionLen = collision.Data.Count - 1;
        // return if the info is empty.
        if (collisionLen <= 0) return true;

        // convert the array from Float32Array to float[]
        float[] collisionArr = new float[collisionLen];
        for (int i = 0; i < collisionLen; i++)
        {
            collisionArr[i] = collision.Data[i + 1];
        }

        // if first float is 1 it's a collison
        if (collision.Data[0] > 0.5f && collision.Data[0] < 1.5 && CageInterface.cageIsConnected)
        {
            CageInterface.Instance.ForwardCollisions(collisionArr);
        }
        // if first float is a 2 it's link information
        else if (collision.Data[0] > 1.5f)
        {
            InitExoforcePublisher.StoreLinkInformation(collisionArr);
        }
#else
        Debug.LogWarning("Collision Modality active while RosSharp is deactivated.");
#endif

        return true;
    }

    /// <summary>
    /// Nothing to do on close, but the method will still be called by animus.
    /// </summary>
    /// <returns>Success.</returns>
    public bool collision_close()
    {
        return true;
    }

    // --------------------------Proprioception Modality----------------------------------
    /// <summary>
    /// Nothing to do on startup, but the method will still be called by animus.
    /// </summary>
    /// <returns>Success.</returns>
    public bool proprioception_initialise()
    {
        return true;
    }

    /// <summary>
    /// Receive body parts information
    /// </summary>
    /// <param name="currSample">
    /// The float array containing the body parts statuses. The parameters are -1.0 for no connection,
    /// 1.0 for connected but a problem with the system/ motor, 0.0 connected and no problem.
    /// </param>
    /// <returns>Success.</returns>
    public bool proprioception_set(Float32Array currSample)
    {
        print("Proprio: " + currSample.Data);
        // check if the float array contains information for the 6 body parts
        if (currSample.CalculateSize() >= 6)
        {
            /* body_manager() handles the information in currSample for the 6 body parts
            mapping: 
            41 id of the head icon, position in currSample 0
            42 id of the right_body icon, position in currSample 1
            43 id of the left_body icon, position in currSample 2
            44 id of the right_hand icon, position in currSample 3
            45 id of the left_hand icon, position in currSample 4
            46 id of the wheelchair icon, position in currSample 5
            */
            body_manager(41, 0, currSample);
            body_manager(42, 1, currSample);
            body_manager(43, 2, currSample);
            body_manager(44, 3, currSample);
            body_manager(45, 4, currSample);
            body_manager(46, 5, currSample);
        }
        return true;
    }

    /// <summary>
    /// Method to handle the information in currSample and control the color of the 6 body parts
    /// </summary>
    /// <param name="id">ID of the icon in the json file</param>
    /// <param name="position">Position of the float in currSample</param>
    /// <param name="currSample">Float array with 6 floats (-1.0; 0.0; 1.0)</param>
    public void body_manager(int id, int position, Float32Array currSample)
    {
        // get the instance of the widget with this id
        Widget widget = Manager.Instance.FindWidgetWithID(id);
        if ((int) (currSample.Data[position]) == -1)
        {
            // float equal to -1 then the widget changes to the icon/color at position 1 in the json file (yellow)
            widget.GetContext().currentIcon = widget.GetContext().icons[1];
        }
        else if ((int) (currSample.Data[position]) == 0)
        {
            // float equal to 0 then the widget changes to the icon/color at position 0 in the json file (green)
            widget.GetContext().currentIcon = widget.GetContext().icons[0];
        }
        else
        {
            // else or equal to 1 the widget changes to the icon/color at position 2 in the json file (red)
            widget.GetContext().currentIcon = widget.GetContext().icons[2];
        }

        // Apply the changes to the instance of the widget with ProcessRosMessage
        widget.ProcessRosMessage(widget.GetContext());
    }


    /// <summary>
    /// Nothing to do on close, but the method will still be called by animus.
    /// </summary>
    /// <returns>Success.</returns>
    public bool proprioception_close()
    {
        return true;
    }

    // --------------------------Motor Modality-------------------------------------
    public bool motor_initialise()
    {
        motorEnabled = true;
        _lastUpdate = 0;
        motorMsg = new Float32Array();
        motorSample = new Sample(DataMessage.Types.DataType.Float32Arr, motorMsg);

        StartCoroutine(SendLEDCommand(LEDS_CONNECTED));
        return true;
    }

    public void EnableMotor(bool enable)
    {
        motorEnabled = enable;
    }

    /// <summary>
    /// Can be used to debug the data send by motor_get.
    /// </summary>
    /// <param name="floatArr">The array that should be printed to the console.</param>
    private static void PrintFloatArray(float[] floatArr)
    {
        string printmsg = "";
        foreach (float f in floatArr)
        {
            printmsg += f + ", ";
        }

        print(printmsg);
    }

    /// <summary>
    /// Send all the necessary data from the simulated roboy to the real roboy.
    /// </summary>
    /// <returns>Success.</returns>
    public Sample motor_get()
    {
        if (!motorEnabled)
        {
            Debug.Log("Motor modality not enabled");
            return null;
        }

        if (Time.time * 1000 - _lastUpdate > 50)
        {
            //Debug.LogError("Motor modality enabled");
            var motorAngles = new List<float>();

            // head joints
            foreach (var segment in _myIKHead.Segments)
            {
                if (segment.Joint != null)
                {
                    motorAngles.Add((float) segment.Joint.X.CurrentValue * Mathf.Deg2Rad);
                }
            }

            // torso joints
            foreach (var segment in _myIKBody.Segments)
            {
                //Debug.Log(segment.name);
                if (segment.Joint != null)
                {
                    motorAngles.Add((float) segment.Joint.X.CurrentValue * Mathf.Deg2Rad);
                }
            }

            // left hand, right hand
            float left_open = 0, right_open = 0;
            if (InputManager.Instance.GetLeftController())
                InputManager.Instance.controllerLeft[0]
                    .TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out left_open);

            if (InputManager.Instance.GetRightController())
                InputManager.Instance.controllerRight[0]
                    .TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out right_open);

            motorAngles.Add(left_open);
            motorAngles.Add(right_open);

            // wheelchair
            Vector2 axis2D;
            if (!WidgetInteraction.settingsAreActive && InputManager.Instance.GetLeftController() &&
                InputManager.Instance.controllerLeft[0]
                    .TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out axis2D))
            {
                motorAngles.Add(axis2D[0]);
                motorAngles.Add(axis2D[1]);
            }
            else
            {
                motorAngles.Add(0);
                motorAngles.Add(0);
            }

            motorMsg.Data.Clear();
            motorMsg.Data.Add(motorAngles);
            motorSample.Data = motorMsg;
            _lastUpdate = Time.time * 1000;

            return motorSample;
        }

        return null;
    }

    public bool motor_close()
    {
        motorEnabled = false;
        StartCoroutine(SendLEDCommand(LEDS_OFF));

        return true;
    }

    private void Update()
    {
        // Enable or disable the displays
        SetDisplaystate();
    }

    // --------------------------Voice Modality----------------------------------
    public bool voice_initialise()
    {
        return voiceEnabled;
    }

    public AudioSample voice_get()
    {
        if (!bodyTransitionReady) return null;
        return null;
    }

    public bool voice_close()
    {
        voiceEnabled = false;
        return true;
    }

    // --------------------------Emotion Modality----------------------------------
    public bool emotion_initialise()
    {
        emotionMsg = new StringSample();
        emotionSample = new Sample(DataMessage.Types.DataType.String, emotionMsg);
        return true;
    }

    // read out the currently pressed button combination and send it as a string via animus
    public Sample emotion_get()
    {
        // convert the button combination to an int
        var controlCombination = ((LeftButton1 ? 1 : 0) * 1) +
                                 ((LeftButton2 ? 1 : 0) * 2) +
                                 ((RightButton1 ? 1 : 0) * 4) +
                                 ((RightButton2 ? 1 : 0) * 8);

        // convert the button combination from an int representation to a string representation
        switch (controlCombination)
        {
            case 0:
                // All off
                currentEmotion = "off";
                break;
            case 1:
                // Left Button 1
                currentEmotion = "A";
                break;
            case 2:
                // Left Button 2
                currentEmotion = "B";
                break;
            case 4:
                // Right Button 1
                currentEmotion = "Y";
                break;
            case 5:
                // Right button 1 and left button 1
                currentEmotion = "AY";
                break;
            case 6:
                currentEmotion = "BY";
                break;
            case 8:
                // Right Button 2
                currentEmotion = "X";
                break;
            case 9:
                currentEmotion = "AX";
                break;
            case 10:
                // Right Button 2 and Left Button 2
                currentEmotion = "BX";
                break;
            default:
                Debug.Log("Unassigned Combination");
                break;
        }


        emotionMsg.Data = currentEmotion;
        if (currentEmotion != "off")
        {
            // Display the current Emotion on the widget
            EmotionManager.Instance.SetFaceByKey(currentEmotion);
        }

        // send the emotion via animus to display it on the real robot
        emotionSample.Data = emotionMsg;
        return emotionSample;
    }

    /// <summary>
    /// Necessary for animus.
    /// </summary>
    /// <returns></returns>
    public bool emotion_close()
    {
        return true;
    }

    // Utilities

    public static Vector3 Vector2Ros(Vector3 vector3)
    {
        return new Vector3(vector3.z, -vector3.x, vector3.y);
    }

    public static Quaternion Quaternion2Ros(Quaternion quaternion)
    {
        return new Quaternion(-quaternion.z, quaternion.x, -quaternion.y, quaternion.w);
    }

    public double ClipAngle(double angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        else if (angle < -180)
        {
            angle += 360;
        }

        return angle;
    }
}
