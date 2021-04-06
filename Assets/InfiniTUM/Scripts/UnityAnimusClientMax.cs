using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using AnimusClient;
using Animus.Data;
using Google.Protobuf.Collections;
#if ANIMUS_USE_OPENCV
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
# endif
using UnityEngine;

public class UnityAnimusClientMax : MonoBehaviour
{
	[Header("Vision Settings")]
    public GameObject visionPlane;
    private Renderer _visionPlaneRenderer;
    private Texture2D _visionTexture;
    private bool _visionEnabled;
    private bool _initMats;
#if ANIMUS_USE_OPENCV
    private Mat _yuv;
    private Mat _rgb;
#endif
    private bool triggerResChange;
    private RepeatedField<uint> _imageDims;

    public static bool sendTransformToCam = true;
    
    private void Start()
    { 
	    
    }
    
    private void Update()
    {
		
    }

    public bool vision_initialise()
    {
	    _visionPlaneRenderer = visionPlane.GetComponent<Renderer>();
	    _visionEnabled = true;
	    _imageDims = new RepeatedField<uint>();
		return _visionEnabled;
	}

	public bool vision_set(ImageSamples currSamples)
	{
		if (!_visionEnabled)
		{
			Debug.Log("Vision modality not enabled. Cannot set");
			return false;
		}

		var currSample = currSamples.Samples[0];
		var currShape = currSample.DataShape;
		
#if ANIMUS_USE_OPENCV
		if (!_initMats)
		{
			_yuv =  new Mat((int)(currShape[1]*1.5), (int)currShape[0] , CvType.CV_8UC1);
			_rgb = new Mat();
			_initMats = true;
		}
		Debug.Log(currSample.FrameNumber);
		_yuv.put(0, 0, currSample.Data.ToByteArray());
		
		Imgproc.cvtColor(_yuv, _rgb, Imgproc.COLOR_YUV2BGR_I420);
		
		if (_imageDims.Count == 0 || currShape[0] != _imageDims[0] || currShape[1] != _imageDims[1] || currShape[2] != _imageDims[2])
        {
	        _imageDims = currShape;
	        var scaleX = (float) _imageDims[0] / (float) _imageDims[1];
	        
	        Debug.Log("Resize triggered. Setting texture resolution to " + currShape[0] + "x" + currShape[1]);
            Debug.Log("Setting horizontal scale to " + scaleX +  " " + (float)_imageDims[0] + " " + (float)_imageDims[1]);
	        
            UnityEngine.Vector3 currentScale = visionPlane.transform.localScale;
            currentScale.x =  scaleX;
            visionPlane.transform.localScale = currentScale;
            
            _visionTexture = new Texture2D(_rgb.width(), _rgb.height(), TextureFormat.ARGB32, false)
            {
                wrapMode = TextureWrapMode.Clamp
            };
        }
		
		//TODO apply stereo images
        Utils.matToTexture2D (_rgb, _visionTexture);
        _visionPlaneRenderer.material.mainTexture = _visionTexture;
#endif
		
		return true;
	}

	public bool vision_close()
	{
		if (!_visionEnabled)
		{
			Debug.Log("Vision modality not enabled. Cannot close");
			return false;
		}
		
		_visionEnabled = false;
		return true;
	}
	
	
	// --------------------------Audition Modality----------------------------------
	public bool audition_initialise()
	{
		return false;
	}

	public bool audition_set(AudioSamples currSample)
	{
		return false;
	}

	public bool audition_close()
	{
		return false;
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
	        
	        InfiniTAMSender.instance.SendData(header, currSample.BytesArray[0].ToByteArray());
	        
    		return true;
    	}
    
    	public bool spatial_close()
    	{
    		return true;
    	}
	
	// --------------------------SpatialSlam Modality----------------------------------
	public bool spatialSlam_initialise()
	{
		return false;
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
		
		//Debug.Log(currSample.Data[0] + ", " + currSample.Data[1] + ", " + currSample.Data[2]);
		return true;
	}

	public bool spatialSlam_close()
	{
		return false;
	}
	
	// --------------------------Motor Modality-------------------------------------
	public bool motor_initialise()
	{
		return false;
	}

	public Float32Array motor_get()
	{
		return null;
	}
        
        //Alternative motor_get call
        //public MotorSample motor_get()
        //{
        //        return null;
        //}

	public bool motor_close()
	{
		return false;
	}


	// --------------------------Voice Modality----------------------------------
	public bool voice_initialise()
	{
		return false;
	}

	public AudioSamples voice_get()
	{
		return null;
	}

	public bool voice_close()
	{
		return false;
	}
	
	// --------------------------Emotion Modality----------------------------------
	public bool emotion_initialise()
	{
		return false;
	}

	public string emotion_get()
	{
		return null;
	}

	public bool emotion_close()
	{
		return false;
	}
}
