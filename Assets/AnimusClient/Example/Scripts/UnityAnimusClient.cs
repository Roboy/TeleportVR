using System;
using System.Collections.Generic;
using System.Linq;
using AnimusClient;
using Animus.Data;
using Google.Protobuf.Collections;
#if ANIMUS_USE_OPENCV
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
# endif
using UnityEngine;

public class UnityAnimusClient : MonoBehaviour
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

	public bool audition_set(AudioSample currSample)
	{
		return false;
	}

	public bool audition_close()
	{
		return false;
	}
	
	// --------------------------Proprioception Modality----------------------------------
	public bool proprioception_initialise()
	{
		return false;
	}

	public bool proprioception_set(float[] currSample)
	{
		return false;
	}

	public bool proprioception_close()
	{
		return false;
	}
	
	// --------------------------Motor Modality-------------------------------------
	public bool motor_initialise()
	{
		return false;
	}

	public float[] motor_get()
	{
		return null;
	}

	public bool motor_close()
	{
		return false;
	}


	// --------------------------Voice Modality----------------------------------
	public bool voice_initialise()
	{
		return false;
	}

	public AudioSample voice_get()
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
