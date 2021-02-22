// using System;
// using System.Collections.Generic;
// using System.Linq;
// using AnimusClient;
// using Animus.Data;
// using Google.Protobuf.Collections;
// #if ANIMUS_USE_OPENCV
// using OpenCVForUnity.CoreModule;
// using OpenCVForUnity.ImgprocModule;
// using OpenCVForUnity.UnityUtils;
// # endif
// using UnityEngine;
// using Widgets;
//
// public class UnityAnimusClient : MonoBehaviour
// {
// 		public GameObject OVRRig;
// // 	public Transform TrackingSpace;
// // 	public GameObject robotBody;
// // 	public Robot chosenDetails;
// 	
// 	[Header("Vision Settings")]
//     public GameObject visionPlane;
//     private Renderer _visionPlaneRenderer;
//     private Texture2D _visionTexture;
//     private bool _visionEnabled;
//     private bool _initMats;
// #if ANIMUS_USE_OPENCV
//     private Mat _yuv;
//     private Mat _rgb;
// #endif
//     private bool triggerResChange;
//     private RepeatedField<uint> _imageDims;
//     
//  //    public GameObject LeftEye;
// 	// public GameObject RightEye;
// 	// private GameObject _leftPlane;
// 	// private GameObject _rightPlane;
// 	// private Renderer _leftRenderer;
// 	// private Renderer _rightRenderer;
// 	// private Texture2D _leftTexture;
// 	// private Texture2D _rightTexture;
// 	// private bool visionEnabled;
// 	// private bool triggerResChange;
// 	// private List<int> imageDims;
// 	// private RepeatedField<uint> _imageDims;
// #if ANIMUS_USE_OPENCV
// 	private Mat yuv;
// 	private Mat rgb;
// #endif
// 	
// 	private bool initMats;
//
//     [Header("Audio Settings")] public bool auditionEnabled = false;
//     
//     [Header("Voice Settings")] public bool voiceEnabled;
//
//     private void Start()
//     { 
// 	    
//     }
//     
//     private void Update()
//     {
// 		
//     }
//
//  //    public bool vision_initialise()
//  //    {
// 	//     _visionPlaneRenderer = visionPlane.GetComponent<Renderer>();
// 	//     _visionEnabled = true;
// 	//     _imageDims = new RepeatedField<uint>();
// 	// 	return _visionEnabled;
// 	// }
//
//     public static void DisplayLatency(float latency, float fps)
//     {
// 	    // TODO: don't mock, save it to the model instead of the view
// 	    Widget latencyTestWidget = Manager.Instance.FindWidgetWithID(33);
// 	    // latencyTestWidget.GetContext().textMessage = "Latency: " + latency + "ms\nFPS: " + fps;
// 	    latencyTestWidget.GetContext().textMessage = $"Latency: {latency:F2}ms\nFPS: {fps:F2}";
// 	    latencyTestWidget.ProcessRosMessage(latencyTestWidget.GetContext());
//     }
//
// // 	public bool vision_set(ImageSamples currSamples)
// // 	{
// // 		if (!_visionEnabled)
// // 		{
// // 			Debug.Log("Vision modality not enabled. Cannot set");
// // 			return false;
// // 		}
// // 		
// // 		print("SampleLen: " + currSamples.Samples.Count);
// //
// // 		var currSample = currSamples.Samples[0];
// // 		var currShape = currSample.DataShape;
// // 		
// // #if ANIMUS_USE_OPENCV
// // 		if (!_initMats)
// // 		{
// // 			_yuv =  new Mat((int)(currShape[1]*1.5), (int)currShape[0] , CvType.CV_8UC1);
// // 			_rgb = new Mat();
// // 			_initMats = true;
// // 		}
// // 		Debug.Log(currSample.FrameNumber);
// // 		_yuv.put(0, 0, currSample.Data.ToByteArray());
// // 		
// // 		Imgproc.cvtColor(_yuv, _rgb, Imgproc.COLOR_YUV2BGR_I420);
// // 		
// // 		if (_imageDims.Count == 0 || currShape[0] != _imageDims[0] || currShape[1] != _imageDims[1] || currShape[2] != _imageDims[2])
// //         {
// // 	        _imageDims = currShape;
// // 	        var scaleX = (float) _imageDims[0] / (float) _imageDims[1];
// // 	        
// // 	        Debug.Log("Resize triggered. Setting texture resolution to " + currShape[0] + "x" + currShape[1]);
// //             Debug.Log("Setting horizontal scale to " + scaleX +  " " + (float)_imageDims[0] + " " + (float)_imageDims[1]);
// // 	        
// //             UnityEngine.Vector3 currentScale = visionPlane.transform.localScale;
// //             currentScale.x =  scaleX;
// //             visionPlane.transform.localScale = currentScale;
// //             
// //             _visionTexture = new Texture2D(_rgb.width(), _rgb.height(), TextureFormat.ARGB32, false)
// //             {
// //                 wrapMode = TextureWrapMode.Clamp
// //             };
// //         }
// // 		
// // 		//TODO apply stereo images
// //         Utils.matToTexture2D (_rgb, _visionTexture);
// //         _visionPlaneRenderer.material.mainTexture = _visionTexture;
// // #endif
// // 		
// // 		return true;
// // 	}
// //
// // 	public bool vision_close()
// // 	{
// // 		if (!_visionEnabled)
// // 		{
// // 			Debug.Log("Vision modality not enabled. Cannot close");
// // 			return false;
// // 		}
// // 		
// // 		_visionEnabled = false;
// // 		return true;
// // 	}
// 	
// 	
// 	public bool vision_initialise()
// 	{
// 		//Get OVR Cameras
// 		var cameras = OVRRig.GetComponentsInChildren<Camera>();
// 		
// 		// Setup ovr camera parameters and attach component transforms to ovr camera transforms
// 		// This allows the planes to follow the cameras
// 		foreach (Camera cam in cameras)
// 		{
// 			Debug.Log("Formatting: " + cam.transform.name);
// 			cam.clearFlags = CameraClearFlags.SolidColor;
// 			cam.backgroundColor = Color.black;
// 			cam.orthographic = true;
// 			cam.orthographicSize = 5;
// 			cam.cullingMask &= ~(1 << 11);
// 			switch (cam.transform.name)
// 			{
// 				case "LeftEyeAnchor":
// 					LeftEye.transform.parent = cam.transform;
// 					LeftEye.transform.localPosition = Vector3.zero;
// 					LeftEye.transform.localEulerAngles = Vector3.zero;
// 					break;
// 				case "RightEyeAnchor":
// 					RightEye.transform.parent = cam.transform;
// 					RightEye.transform.localPosition = Vector3.zero;
// 					RightEye.transform.localEulerAngles = Vector3.zero;
// 					break;
// 			}
// 		}
//
// 		_leftPlane = LeftEye.transform.Find("LeftEyePlane").gameObject;
// 		_rightPlane = RightEye.transform.Find("RightEyePlane").gameObject;
//
// 		_leftRenderer = _leftPlane.GetComponent<Renderer>();
// 		_rightRenderer = _rightPlane.GetComponent<Renderer>();
// 		_imageDims = new RepeatedField<uint>();
// 		visionEnabled = true;
// 		
// 		// Comment the line below to enable two images - Not tested
// 		RightEye.SetActive(false);
// 		return visionEnabled;
// 	}
//
// 	public bool vision_set(ImageSamples currSamples)
// 	{
// 	
// 	    try
// 	    {
// 		if (!bodyTransitionReady) return true;
// 		
// 		if (!visionEnabled)
// 		{
// 			Debug.Log("Vision modality not enabled. Cannot set");
// 			return false;
// 		}
//
// 		if (currSamples == null)
// 		{
// 			return false;
// 		}
// 		
// 		print("SampleLen: " + currSamples.Samples.Count);
//
// 		var currSample = currSamples.Samples[0];
// 		
// 		
// 			var currShape = currSample.DataShape;
// 			// Debug.Log($"{currShape[0]}, {currShape[1]}");
// #if ANIMUS_USE_OPENCV
// 			if (!initMats)
// 			{
// 				yuv =  new Mat((int)(currShape[1]*1.5), (int)currShape[0] , CvType.CV_8UC1);
// 				rgb = new Mat();
// 				initMats = true;
// 			}
// 			
// 			if (currSample.Data.Length != currShape[0] * currShape[1] * 1.5)
// 			{
// 				return true;
// 			}
// 			
// 			if (currShape[0] <= 100 || currShape[1] <= 100)
// 			{
// 				return true;
// 			}
// 			// Debug.Log("cvt Color ops");
// 			
// 			yuv.put(0, 0, currSample.Data.ToByteArray());
// 			
// 			Imgproc.cvtColor(yuv, rgb, Imgproc.COLOR_YUV2BGR_I420);
// 			
// 			if (_imageDims.Count == 0 || currShape[0] != _imageDims[0] || currShape[1] != _imageDims[1] || currShape[2] != _imageDims[2])
// 	        {
// 		        _imageDims = currShape;
// 		        var scaleX = (float) _imageDims[0] / (float) _imageDims[1];
// 		        
// 		        Debug.Log("Resize triggered. Setting texture resolution to " + currShape[0] + "x" + currShape[1]);
// 	            Debug.Log("Setting horizontal scale to " + scaleX +  " " + (float)_imageDims[0] + " " + (float)_imageDims[1]);
// 	            
// 	            UnityEngine.Vector3 currentScale = _leftPlane.transform.localScale;
// 	            currentScale.x =  scaleX;
//
// 	            _leftPlane.transform.localScale = currentScale;
// 	            _leftTexture = new Texture2D(rgb.width(), rgb.height(), TextureFormat.ARGB32, false)
// 	            {
// 	                wrapMode = TextureWrapMode.Clamp
// 	            };
// 	            
// 	            // _rightPlane.transform.localScale = currentScale;
// 	            // _rightTexture = new Texture2D(rgb.width(), rgb.height(), TextureFormat.ARGB32, false)
// 	            // {
// 		           //  wrapMode = TextureWrapMode.Clamp
// 	            // };
// // 	            return true;
// 	        }
// 		// Debug.Log("matToTexture2D");
// 			
// 			//TODO apply stereo images
// 	        Utils.matToTexture2D (rgb, _leftTexture);
// 	        _leftRenderer.material.mainTexture = _leftTexture;
// #endif
// 		}
// 		catch (Exception e)
// 		{
// 			Debug.Log(e);
// 		}
// // 		Debug.Log("done vision set");
// 		return true;
// 	}
//
// 	public bool vision_close()
// 	{
// 		if (!visionEnabled)
// 		{
// 			Debug.Log("Vision modality not enabled. Cannot close");
// 			return false;
// 		}
// 		
// 		visionEnabled = false;
// 		return true;
// 	}
//
// 	// --------------------------Audition Modality----------------------------------
// 	public bool audition_initialise()
// 	{
// 		return auditionEnabled;
// 	}
//
// 	public bool audition_set(AudioSample currSample)
// 	{
// 		print("AudioSample: " + currSample);
// 		return auditionEnabled;
// 	}
//
// 	public bool audition_close()
// 	{
// 		auditionEnabled = false;
// 		return true;
// 	}
// 	
// 	// --------------------------Proprioception Modality----------------------------------
// 	public bool proprioception_initialise()
// 	{
// 		return false;
// 	}
//
// 	public bool proprioception_set(float[] currSample)
// 	{
// 		return false;
// 	}
//
// 	public bool proprioception_close()
// 	{
// 		return false;
// 	}
// 	
// 	// --------------------------Motor Modality-------------------------------------
// 	public bool motor_initialise()
// 	{
// 		return false;
// 	}
//
// 	public float[] motor_get()
// 	{
// 		return null;
// 	}
//
// 	public bool motor_close()
// 	{
// 		return false;
// 	}
//
//
// 	// --------------------------Voice Modality----------------------------------
// 	public bool voice_initialise()
// 	{
// 		return voiceEnabled;
// 	}
//
// 	public AudioSample voice_get()
// 	{
// 		//if (!bodyTransitionReady) return null;
// 		return null;
// 	}
//
// 	public bool voice_close()
// 	{
// 		voiceEnabled = false;
// 		return true;
// 	}
// 	
// 	// --------------------------Emotion Modality----------------------------------
// 	public bool emotion_initialise()
// 	{
// 		return false;
// 	}
//
// 	public string emotion_get()
// 	{
// 		return null;
// 	}
//
// 	public bool emotion_close()
// 	{
// 		return false;
// 	}
// }


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animus.Data;
using Animus.RobotProto;
using AnimusCommon;
using Google.Protobuf.Collections;
// using BioIK;
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

public class UnityAnimusClient : MonoBehaviour {
	
	public GameObject OVRRig;
	public Transform TrackingSpace;
	public GameObject robotBody;
	public Robot chosenDetails;
	
	// vision variables
	public GameObject LeftEye;
	public GameObject RightEye;
	private GameObject _leftPlane;
	private GameObject _rightPlane;
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
	// private BioIK.BioIK _myIKBody;
	// private List<BioSegment> _actuatedJoints;
	private bool motorEnabled;
	private float _lastUpdate;
	
	private bool bodyTransitionReady;
	private int bodyTransitionDuration = 1;
	
	private Animus.Data.Float32Array motorMsg;
	private Sample motorSample;
	
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
		//humanLeftHand = TrackingSpace.Find("LeftHandAnchor");
		//humanRightHand = TrackingSpace.Find("RightHandAnchor"); 

		// LeftEye = this.transform.Find("LeftEye").gameObject;
		// RightEye = this.transform.Find("RightEye").gameObject;
		
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
		print("BodyTransition ready");
	}
	
	 IEnumerator SendLEDCommand(string command) {
	 	 using (UnityWebRequest webRequest = UnityWebRequest.Get("https://lib.roboy.org/teleportal/" + command))
		  {
		   webRequest.certificateHandler = new BypassCertificate();
		   // Request and wait for the desired page.
		   yield return webRequest.SendWebRequest();
	   }
	}

	// --------------------------Vision Modality----------------------------------
	public static void DisplayLatency(float latency, float fps)
	{
		// Present the latency and fps
		Widget latencyWidget = Manager.Instance.FindWidgetWithID(33);
		if (latency < 0) {
			latencyWidget.GetContext().textMessage = $"FPS: {fps:F2}";
		}
		else 
		{
        	latencyWidget.GetContext().textMessage = $"Latency: {latency:F2}ms\nFPS: {fps:F2}";
		}
		print($"Latency: {latency:F2}ms\nFPS: {fps:F2}");
        //latencyWidget.GetContext().graphTimestamp = Time.time;
        //latencyWidget.GetContext().graphValue = latency;
        latencyWidget.ProcessRosMessage(latencyWidget.GetContext());
		
		// turn the icon from yellow (no connection) to green
		Widget wifiWidget = Manager.Instance.FindWidgetWithID(23);
		wifiWidget.GetContext().currentIcon = "WifiGreen";
		wifiWidget.ProcessRosMessage(wifiWidget.GetContext());
	}
	
	public bool vision_initialise()
	{
		//Get OVR Cameras
		var cameras = OVRRig.GetComponentsInChildren<Camera>();
		
		// Setup ovr camera parameters and attach component transforms to ovr camera transforms
		// This allows the planes to follow the cameras
		foreach (Camera cam in cameras)
		{
			Debug.Log("Formatting: " + cam.transform.name);
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

		_leftPlane = LeftEye.transform.Find("LeftEyePlane").gameObject;
		_rightPlane = RightEye.transform.Find("RightEyePlane").gameObject;

		_leftRenderer = _leftPlane.GetComponent<Renderer>();
		_rightRenderer = _rightPlane.GetComponent<Renderer>();
		_imageDims = new RepeatedField<uint>();
		visionEnabled = true;
		
		// Comment the line below to enable two images - Not tested
		//RightEye.SetActive(false);
		return visionEnabled;
	}

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
		//currShape[1] /= 2;
		
		//for (int i = 0; i < 2; i++)
		//{

			var all_bytes = currSample.Data.ToByteArray();
			//byte[] bytes;
			//if (i == 0)
			//{
			//	bytes = all_bytes.Take(all_bytes.Length / 2).ToArray();
			//}
			//else
			//{
			//	bytes = all_bytes.Skip(all_bytes.Length / 2).ToArray();
			//}

			Debug.Log($"{currShape[0]}, {currShape[1]}");
#if ANIMUS_USE_OPENCV
			if (!initMats)
			{
				yuv = new Mat((int) (currShape[1] * 1.5), (int) currShape[0], CvType.CV_8UC1);
				rgb = new Mat();
				initMats = true;
			}

			//if (currSample.Data.Length != currShape[0] * currShape[1] * 1.5)
			if (all_bytes.Length != currShape[0] * currShape[1] * 1.5)
			{
				return true;
			}

			if (currShape[0] <= 100 || currShape[1] <= 100)
			{
				return true;
			}
			// Debug.Log("cvt Color ops");

			//yuv.put(0, 0, currSample.Data.ToByteArray());
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

				_leftPlane.transform.localScale = currentScale;
				//_leftTexture = new Texture2D(rgb.width(), rgb.height(), TextureFormat.ARGB32, false)
				_leftTexture = new Texture2D(rgb.width(), rgb.height() / 2, TextureFormat.ARGB32, false)
				{
					wrapMode = TextureWrapMode.Clamp
				};

				_rightPlane.transform.localScale = currentScale;
				//_rightTexture = new Texture2D(rgb.width(), rgb.height(), TextureFormat.ARGB32, false)
				_rightTexture = new Texture2D(rgb.width(), rgb.height() / 2, TextureFormat.ARGB32, false)
				{
					wrapMode = TextureWrapMode.Clamp
				};
// 	            return true;
			}
			// Debug.Log("matToTexture2D");
			
			for (int i = 0; i < 2; i++)
			{

			//TODO apply stereo images
			//if (currSample.Source == "LeftCamera")
			if (i == 0)
			{
				Mat rgb_l = rgb.rowRange(0, rgb.rows()/2);
				Utils.matToTexture2D(rgb_l, _leftTexture);
				_leftRenderer.material.mainTexture = _leftTexture;
			}
			//else if (currSample.Source == "RightCamera")
			else if (i == 1)
			{
				Mat rgb_r = rgb.rowRange(rgb.rows()/2, rgb.rows());
				Utils.matToTexture2D(rgb_r, _rightTexture);
				_rightRenderer.material.mainTexture = _rightTexture;
			}
			else
			{
				print("Unknown image source: " + currSample.Source);
			}
		}
#endif
        }
        catch (Exception e)
		{
			Debug.Log(e);
		}
// 		Debug.Log("done vision set");
		return true;
	}

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
	
	
	// --------------------------Audition Modality----------------------------------
	public bool audition_initialise()
	{
		return auditionEnabled;
	}

	public bool audition_set(AudioSample currSample)
	{
		print("AudioSample: " + currSample);

		if (!bodyTransitionReady) return true;
		return auditionEnabled;
	}

	public bool audition_close()
	{
		auditionEnabled = false;
		return true;
	}
	
	// --------------------------Proprioception Modality----------------------------------
	public bool proprioception_initialise()
	{
		return true;
	}

	public bool proprioception_set(Float32Array currSample)
	{
		//print(currSample);
		
		Widget headWidget = Manager.Instance.FindWidgetWithID(41);
		Widget rightBodyWidget = Manager.Instance.FindWidgetWithID(42);
		Widget leftBodyWidget = Manager.Instance.FindWidgetWithID(43);
		Widget rightHandWidget = Manager.Instance.FindWidgetWithID(44);
		Widget leftHandWidget = Manager.Instance.FindWidgetWithID(45);
		Widget wheelchairWidget = Manager.Instance.FindWidgetWithID(46);

		headWidget.GetContext().currentIcon = currSample.ToString()[12] == '0' ? "HeadGreen" : "HeadRed";
		rightBodyWidget.GetContext().currentIcon = currSample.ToString()[15] == '0' ? "RightBodyGreen" : "RightBodyRed";
		leftBodyWidget.GetContext().currentIcon = currSample.ToString()[18] == '0' ? "LeftBodyGreen" : "LeftBodyRed";
		rightHandWidget.GetContext().currentIcon = currSample.ToString()[21] == '0' ? "RightHandGreen" : "RightHandRed";
		leftHandWidget.GetContext().currentIcon = currSample.ToString()[24] == '0' ? "LeftHandGreen" : "LeftHandRed";
		wheelchairWidget.GetContext().currentIcon = currSample.ToString()[27] == '0' ? "WheelchairGreen" : "WheelchairRed";

		headWidget.ProcessRosMessage(headWidget.GetContext());
		rightBodyWidget.ProcessRosMessage(rightBodyWidget.GetContext());
		leftBodyWidget.ProcessRosMessage(leftBodyWidget.GetContext());
		rightHandWidget.ProcessRosMessage(rightHandWidget.GetContext());
		leftHandWidget.ProcessRosMessage(leftHandWidget.GetContext());
		wheelchairWidget.ProcessRosMessage(wheelchairWidget.GetContext());

		/*
		if (currSample.CalculateSize() > 1)
		{
 			if (currSample[0]>0) { 
	            OVRInput.SetControllerVibration(currSample[0], currSample[1], OVRInput.Controller.LTouch);
			// TODO: maybe add vibration as well?
 			}
		}
		*/
		
		return true;
	}

	public bool proprioception_close()
	{
		return true;
	}
	
	// --------------------------Motor Modality-------------------------------------
	/*public bool motor_initialise()
	{
		motorEnabled = true;
		_lastUpdate = 0;
		motorMsg = new Float32Array();
		motorSample = new Sample(DataMessage.Types.DataType.Float32Arr, motorMsg);

		StartCoroutine(SendLEDCommand(LEDS_CONNECTED));
		return true;
	}

	public Sample motor_get()
	{
		if (!bodyTransitionReady) return null;
		if (!motorEnabled)
		{
			Debug.Log("Motor modality not enabled");
			return null;
		}

		if (Time.time * 1000 - _lastUpdate > 50)
		{
			var headAngles = humanHead.eulerAngles;
			var roll = ClipAngle(headAngles.x);
			var pitch = ClipAngle(-headAngles.y);
			var yaw = ClipAngle(headAngles.z);
			
			var motorAngles = new List<float>
			{
				0, 0,
				(float)roll * Mathf.Deg2Rad,
				-(float)pitch * Mathf.Deg2Rad,
				(float) yaw * Mathf.Deg2Rad,
			};

// // 			if (trackingLeft)
// // 			{
				// TODO: replace line below with Unity XR code
				// motorAngles.Add(OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger));
// 				motorAngles.Add(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch));
				robotLeftHandPositionROS = Vector2Ros(humanLeftHand.position);
				robotLeftHandOrientationROS = Quaternion2Ros(Quaternion.Euler(humanLeftHand.eulerAngles));
				motorAngles.AddRange(new List<float>()
				{
					robotLeftHandPositionROS.x,
					robotLeftHandPositionROS.y,
					robotLeftHandPositionROS.z,
					robotLeftHandOrientationROS.x,
					robotLeftHandOrientationROS.y,
					robotLeftHandOrientationROS.z,
					robotLeftHandOrientationROS.w
					// Add other robot angles here
					// leftHandClosed
				});
// // 			} else {
				
// // 				motorAngles.Add(0.0f);
// // 				motorAngles.AddRange( new List<float>(){0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f});
// // 			}

// // 			if (trackingRight)
// // 			{
				// TODO: replace line below with Unity XR code
				//motorAngles.Add(OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger));
// 				motorAngles.Add(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch));
				robotRightHandPositionROS = Vector2Ros(humanRightHand.position);
				robotRightHandOrientationROS = Quaternion2Ros(Quaternion.Euler(humanRightHand.eulerAngles));
				motorAngles.AddRange(new List<float>()
				{
					robotRightHandPositionROS.x,
					robotRightHandPositionROS.y,
					robotRightHandPositionROS.z,
					robotRightHandOrientationROS.x,
					robotRightHandOrientationROS.y,
					robotRightHandOrientationROS.z,
					robotRightHandOrientationROS.w
				});
				
				
// 			} else {
				if (RightButton1) motorAngles.Add(2000.0f);
// 				motorAngles.Add(0.0f);
// 				motorAngles.AddRange( new List<float>(){0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f});
// 			}
			
// 				motorAngles.Add(1.0f);
// 				robotHeadPositionROS = Vector2Ros(humanHead.position);
// 				robotHeadOrientationROS = Quaternion2Ros(Quaternion.Euler(humanHead.eulerAngles));
// 				motorAngles.AddRange(new List<float>()
// 				{
// 					robotHeadPositionROS.x,
// 					robotHeadPositionROS.y,
// 					robotHeadPositionROS.z,
// 					robotHeadOrientationROS.x,
// 					robotHeadOrientationROS.y,
// 					robotHeadOrientationROS.z,
// 					robotHeadOrientationROS.w
// 				});
				
// 				eyesPosition = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
// 				motorAngles.Add(eyesPosition[0]);
// 				motorAngles.Add(eyesPosition[1]);

			motorMsg.Data.Clear();
			motorMsg.Data.Add(motorAngles);
			motorSample.Data = motorMsg;
	
			return motorSample;
		
		}

		return null;
		
		// var headAngles = humanHead.eulerAngles;
		// for (var i = 0; i < 3; i++)
		// {
		// 	if (headAngles[i] > 180)
		// 	{
		// 		headAngles[i] -= 360;
		// 	}
		// }
		
		// // Primary is always left and Secondary is right
		// var rightJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
		// var leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
		
		
		// //Use y value for X vel and Y value for X vel
		// var currLocomotion = new Vector3(leftJoystick.y, -leftJoystick.x, -rightJoystick.x);
		// var leftHandClosed = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		// var rightHandClosed = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
		
		// // Assemble float array. This is specific to each robot for now. But with BioIK on the robot side can be generalised
		// var motorAngles = new List<float>
		// {
		// 	-headAngles.y,
		// 	headAngles.x,
		// 	headAngles.z,
		// 	currLocomotion.x, 
		// 	currLocomotion.y,
		// 	currLocomotion.z,
		// 	-1.0f,
		// 	-1.0f,
		// };

		// if (robotDriver != null)
		// {
		// 	var virtualRobotJointAngles = SampleBioJoints();
		// 	if (trackingLeft)
		// 	{
		// 		motorAngles[6] = 1.0f;
		// 		motorAngles.AddRange(new List<float>()
		// 		{
		// 			// Add other robot angles here
		// 			leftHandClosed
		// 		});
		// 	}

		// 	if (trackingRight)
		// 	{
		// 		motorAngles[7] = 1.0f;
		// 		motorAngles.AddRange(new List<float>()
		// 		{
		// 			// Add other robot angles here
		// 			rightHandClosed
		// 		});
		// 	}
		// }

		// return motorAngles.ToArray();
	}

	public bool motor_close()
	{
		motorEnabled = false;
		StartCoroutine(SendLEDCommand(LEDS_OFF));
		return true;
	}*/

	// --------------------------Motor Modality-------------------------------------
  public bool motor_initialise()
  {
    motorEnabled = true;
    _lastUpdate = 0;
    motorMsg = new Float32Array();
    motorSample = new Sample(DataMessage.Types.DataType.Float32Arr, motorMsg);

    
    return true;
  }

  public Sample motor_get()
  {
    if (!bodyTransitionReady) return null;
    if (!motorEnabled)
    {
      Debug.Log("Motor modality not enabled");
      return null;
    }

    if (Time.time * 1000 - _lastUpdate > 50)
    {
      var headAngles = humanHead.eulerAngles;
      var roll = ClipAngle(headAngles.x);
      var pitch = ClipAngle(-headAngles.y);
      var yaw = ClipAngle(headAngles.z);
      
      var motorAngles = new List<float>
      {
        0, 0,
        (float)roll * Mathf.Deg2Rad,
        -(float)pitch * Mathf.Deg2Rad,
        (float) yaw * Mathf.Deg2Rad,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
      };

	  motorMsg.Data.Clear();
      motorMsg.Data.Add(motorAngles);
      motorSample.Data = motorMsg;
	  
    	return motorSample;
	}

    return null;
	}

	public bool motor_close()
  	{
    	motorEnabled = false;
    
    return true;
  	}
	
	private void FixedUpdate()
	{
		// TODO: replace lines below with Unity XR code
		// LeftButton1 = OVRInput.GetDown(OVRInput.Button.One);
		// LeftButton2 = OVRInput.GetDown(OVRInput.Button.Two);
		// RightButton1 = OVRInput.GetDown(OVRInput.Button.Four);
		// RightButton2 = OVRInput.GetDown(OVRInput.Button.Three);
	}

	private void Update()
	{

		if (motorEnabled && bodyTransitionReady)
		{
			
			
			// move robot wherever human goes
			//bodyToBaseOffset = robotBase.position - robotBody.transform.position;
			//robotBody.transform.position = humanHead.position - bodyToBaseOffset;

			// TODO: replace lines below with Unity XR code
// 			if (robotDriver != null)
// 			{
// // 				if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0)
// // 				{
// 					robotLeftHandObjective.position = humanLeftHand.position;
// 					robotLeftHandObjective.eulerAngles = humanLeftHand.eulerAngles;
// 					humanLeftHandOpen = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTrackedRemote);
// 					trackingLeft = true;
// // 				}
// // 				else
// // 				{
// // 					trackingLeft = false;
// // 				}
// 			
// // 				if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0)
// // 				{
// 					robotRightHandObjective.position = humanRightHand.position;
// 					robotRightHandObjective.eulerAngles = humanRightHand.eulerAngles;
// 					humanRightHandOpen = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTrackedRemote);
// 					trackingRight = true;
// // 				}
// // 				else
// // 				{
// // 					trackingRight = false;
// // 				}
//
// 				
// 			}
		}
		
		
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

	public Sample emotion_get()

	{
		return null;
		
// 		var controlCombination = ((LeftButton1 ? 1 : 0) * 1) + 
// 		                         ((LeftButton2 ? 1 : 0) * 2) +
// 		                         ((RightButton1 ? 1 : 0) * 4) +
// 		                         ((RightButton2 ? 1 : 0) * 8);

// 		switch (controlCombination)
// 		{
// 			case 0:
// 				// All off
// 				currentEmotion = "off";
// 				break;
// 			case 1:
// 				// Left Button 1
// 				currentEmotion = "A";
// 				break;
// 			case 2:
// 				// Left Button 2
// 				currentEmotion = "B";
// 				break;
// 			case 4:
// 				// Right Button 1
// 				currentEmotion = "X";
// 				break;
// 			case 8:
// 				// Right Button 2
// 				currentEmotion = "Y";
// 				break;
// 			case 10:
// 				// Right Button 2 and Left Button 2
// 				currentEmotion = "BY";
// 				break;
// 			default:
// 				Debug.Log("Unassigned Combination");
// 				break;
// 		}
		

// 		emotionMsg.Data = currentEmotion;
// 		Debug.Log(currentEmotion);
// 		emotionSample.Data = emotionMsg;
// 		return emotionSample;

// // 		if (!bodyTransitionReady) return null;
// // 		if (oldEmotion != currentEmotion)
// // 		{
// // 			Debug.Log(currentEmotion);
// // 			oldEmotion = currentEmotion;
// // 			return currentEmotion;
// // 		}
// // 		else
// // 		{
// // 			return null;
// // 		}
	}

	public bool emotion_close()
	{
		return true;
	}

	// Utilities

	public Vector3 Vector2Ros(Vector3 vector3)
    {
        return new Vector3(vector3.z, -vector3.x, vector3.y);
    }

	public Quaternion Quaternion2Ros(Quaternion quaternion)
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
