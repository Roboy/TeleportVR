using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour {
    private CustomSlider slider;
    private AudioSource song;
    
    /// <summary>
    /// Get reference to instances
    /// </summary>
    void Start() {
        slider = GameObject.FindObjectOfType<CustomSlider>();
        song = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update Volume value of the song according to the current slider value
    /// </summary>
    void Update() {
        song.volume = slider.GetValue();
    }
}
