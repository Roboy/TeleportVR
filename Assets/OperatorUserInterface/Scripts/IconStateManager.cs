using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconStateManager : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [Tooltip("The panel that should be shown when looking at this button")]
    public GameObject DetailedPanel;
    
    [Tooltip("The image which should blink when there is a notification")]
    public Image GlowEffect;

    private float glowPulseDuration = 5.5f;
    private float glowPulseTimer = -1;

    [Tooltip("The time the user needs to look at the thumbnail until the detailed panel gets shown")]
    public float dwellTimeActivate = 0.5f;
    private float timerActivate;
    
    [Tooltip("The time the user needs to look at the thumbnail until the detailed panel gets shown")]
    public float dwellTimeDeactivate = 0.5f;
    private float timerDeactivate;
    private bool lookingAway = true;


    // Start is called before the first frame update
    void Start()
    {
        if (DetailedPanel == null)
        {
            Debug.LogWarning("Pls add a panel which gets shown when the Thumbnail " + gameObject.name + 
                             " gets looked at!");
        }

        timerActivate = dwellTimeActivate;
        timerDeactivate = dwellTimeDeactivate;
        
        SetGlowAlph(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (lookingAway)
        {
            timerDeactivate -= Time.deltaTime;
            if (timerDeactivate <= 0 && DetailedPanel.activeSelf)
            {
                DeactivateDetails();
            }
        }
        else
        {
            timerActivate -= Time.deltaTime;
            if (timerActivate <= 0 && !DetailedPanel.activeSelf)
            {
                ActivateDetails();
            }
        }

        if (glowPulseTimer >= 0 && glowPulseTimer < glowPulseDuration)
        {
            SetGlowAlph(1 - 0.5f * ((float)Math.Cos(glowPulseTimer * Math.PI) + 1));
            glowPulseTimer += Time.deltaTime;
        } else if (glowPulseTimer >= glowPulseDuration)
        {
            SetGlowAlph(0.5f);
        }
        else
        {
            float currAlph = GlowEffect.color.a;
            if (currAlph > 0)
            {
                SetGlowAlph(currAlph -= Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Sets the alpha value of the glow effect, e.g. setting it to 0 will make it invisible.
    /// </summary>
    /// <param name="alphaVal"></param>
    private void SetGlowAlph(float alphaVal)
    {
        Color glowCol = GlowEffect.color;
        glowCol.a = alphaVal;
        GlowEffect.color = glowCol;
    }

    public void ShowNotification()
    {
        SetGlowAlph(1);
        glowPulseTimer = 0;
    }
    
    public void StopNotification()
    {
        //SetGlowAlph(0);
        glowPulseTimer = -1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        lookingAway = false;
        timerActivate = dwellTimeActivate;
        timerDeactivate = dwellTimeDeactivate;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lookingAway = true;
        timerActivate = dwellTimeActivate;
        timerDeactivate = dwellTimeDeactivate;
    }

    
    public void OnButtonPressed()
    {
        if (DetailedPanel != null)
        {
            if (DetailedPanel.activeSelf)
            {
                DeactivateDetails();
            }
            else
            {
                ActivateDetails();
            }
        }
    }

    // TODO: Make fancy (de)activation animation
    public void ActivateDetails()
    {
        DetailedPanel.SetActive(true);
        ShowNotification();
    }
    
    public void DeactivateDetails()
    {
        DetailedPanel.SetActive(false);
        StopNotification();
    }
}
