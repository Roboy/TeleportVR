using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzManager : MonoBehaviour
{
    public bool RightHand;
    public bool LeftHand;
    private int[] fingersRight;
    private int[] fingersLeft;
//#if SENSEGLOVE
//    private SenseGlove_Object senseGloveObjectRight;
//    private SenseGlove_Object senseGloveObjectLeft;
//#endif

    /// <summary>
    /// Set reference to instances
    /// </summary>
    void OnEnable()
    {
//#if SENSEGLOVE
//        if (RightHand)
//        {
//            senseGloveObjectRight = GameObject.FindGameObjectWithTag("SenseGloveRight").transform.GetChild(0).GetComponent<SenseGlove_Object>();
//            fingersRight = new int[] { 0, 0, 0, 0, 0 };
//        }
//        if (LeftHand)
//        {
//            senseGloveObjectLeft = GameObject.FindGameObjectWithTag("SenseGloveLeft").transform.GetChild(0).GetComponent<SenseGlove_Object>();
//            fingersLeft = new int[] { 0, 0, 0, 0, 0 };
//        }
//#endif
    }

    /// <summary>
    /// Executes the queued buzz requests (stored in fingersRight/fingersLeft array).
    /// Resets array, because requests are successfully carried out.
    /// </summary>
    void Update()
    {
//#if SENSEGLOVE
//        if (RightHand)
//        {
//            senseGloveObjectRight.SendBuzzCmd(fingersRight, 500);
//            fingersRight = new int[] { 0, 0, 0, 0, 0 };
//        }
//        if (LeftHand)
//        {
//            senseGloveObjectLeft.SendBuzzCmd(fingersLeft, 500);
//            fingersLeft = new int[] { 0, 0, 0, 0, 0 };
//        }
//#endif
    }

    /// <summary>
    /// Public method to schedule a request to buzz certain fingers with a specified intensity.
    /// </summary>
    /// <param name="rightHand">Specifies which hand's finger shall buzz</param>
    /// <param name="fingerindex">Specifies the finger that shall buzz (0: thumb - 4: pinky)</param>
    /// <param name="buzzintensity">The intensity, how strong shall the motor start buzzing</param>
    public void ActivateFinger(bool rightHand, int fingerindex, int buzzintensity)
    {
        if(fingerindex < 5 && fingerindex >= 0)
        {
            if (rightHand)
            {
                fingersRight[fingerindex] = buzzintensity;
            }
            else
            {
                fingersLeft[fingerindex] = buzzintensity;
            }
        }
    }
}
