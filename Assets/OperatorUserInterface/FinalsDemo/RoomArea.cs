using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using Valve.VR;

public class RoomArea : MonoBehaviour
{
    public float speedRot;
    public float speedPos;

    bool moveForward;
    bool rotateLeft;
    bool rotateRight;

    Transform cameraOrigin;

    private bool IsDirty;
    private Vector3 oldPos;

    void Start()
    {
        moveForward = false;
        rotateRight = false;
        rotateLeft = false;

        IsDirty = false;
        oldPos = this.transform.position;

        cameraOrigin = GameObject.FindGameObjectWithTag("CameraOrigin").transform;
    }

    private void Update()
    {
        

        if (rotateRight)
        {
            cameraOrigin.localEulerAngles = new Vector3(0f, cameraOrigin.localEulerAngles.y + Time.deltaTime * speedRot, 0f);
        }
        else if (rotateLeft)
        {
            cameraOrigin.localEulerAngles = new Vector3(0f, cameraOrigin.localEulerAngles.y - Time.deltaTime * speedRot, 0f);
        }
        else if (moveForward)
        {
            Vector3 newPos = transform.position + transform.forward * Time.deltaTime * speedPos;
            if (validatePositionInRoom(newPos))
            {
                //cameraOrigin.position = newPos;//this.transform.position = newPos;
            }
        }
        /*else
        {
            Vector3 updatedPos = cameraOrigin.transform.position + (this.transform.position - oldPos);
            if (validatePositionInRoom(updatedPos))
            {
                cameraOrigin.transform.position = updatedPos;
            }
            oldPos = this.transform.position;
        }*/

        
    }
    
    // TODO: replace code below with 

    
#region move by controller
    /*
    public SteamVR_Action_Boolean TriggerClick;
    public SteamVR_Action_Boolean LeftClick;
    public SteamVR_Action_Boolean RightClick;
    private SteamVR_Input_Sources inputSource;

    /// <summary>
    /// Hook up listeners for controller input
    /// </summary>
    private void OnEnable()
    {
        TriggerClick.AddOnStateDownListener(TriggerDown, inputSource);
        LeftClick.AddOnStateDownListener(LeftDown, inputSource);
        RightClick.AddOnStateDownListener(RightDown, inputSource);

        TriggerClick.AddOnStateUpListener(TriggerUp, inputSource);
        LeftClick.AddOnStateUpListener(LeftUp, inputSource);
        RightClick.AddOnStateUpListener(RightUp, inputSource);
    }

    /// <summary>
    /// Release listeners for controller input
    /// </summary>
    private void OnDisable()
    {
        TriggerClick.RemoveOnStateDownListener(TriggerDown, inputSource);
        LeftClick.RemoveOnStateDownListener(LeftDown, inputSource);
        RightClick.RemoveOnStateDownListener(RightDown, inputSource);

        TriggerClick.RemoveOnStateUpListener(TriggerUp, inputSource);
        LeftClick.RemoveOnStateUpListener(LeftUp, inputSource);
        RightClick.RemoveOnStateUpListener(RightUp, inputSource);
    }

    /// <summary>
    /// Logic being executed if trigger is down
    /// </summary>
    /// <param name="fromAction">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    /// <param name="fromSource">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        moveForward = true;
    }

    /// <summary>
    /// Logic being executed if trigger is up
    /// </summary>
    /// <param name="fromAction">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    /// <param name="fromSource">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    private void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        moveForward = false;
    }

    /// <summary>
    /// Logic being executed if trackpad left is down
    /// </summary>
    /// <param name="fromAction">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    /// <param name="fromSource">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    private void LeftDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        rotateLeft = true;
    }

    /// <summary>
    /// Logic being executed if trackpad left is up
    /// </summary>
    /// <param name="fromAction">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    /// <param name="fromSource">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    private void LeftUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        rotateLeft = false;
    }

    /// <summary>
    /// Logic being executed if trackpad right is down
    /// </summary>
    /// <param name="fromAction">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    /// <param name="fromSource">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    private void RightDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        rotateRight = true;
    }

    /// <summary>
    /// Logic being executed if trackpad right is up
    /// </summary>
    /// <param name="fromAction">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    /// <param name="fromSource">see SteamVR documentation eg. AddOnStateUpListener parameters</param>
    private void RightUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        rotateRight = false;
    }
    */
#endregion

#region room scale
    /// <summary>
    /// Checks wheter a given position is within the room.
    /// Only checks for floor plan, y-value is neglected.
    /// </summary>
    /// <param name="pos">The position that needs validation</param>
    /// <returns>bool is the position within room?</returns>
    public bool validatePositionInRoom(Vector3 pos)
    {
        float x = pos.x;
        float z = pos.z;

        //check if pos is within the room
        //the room is split into 4 rectangles
        if (x > -4.5f && x < 4.5f && z > -34.5 && z < 4.5)
        {
            return true;
        }

        if (x > 4.5f && x < 34.5f && z > -34.5 && z < -25.5)
        {
            return true;
        }

        if (x > 4.5f && x < 14.5f && z > -14.5 && z < -5.5)
        {
            return true;
        }

        if (x > 15.5f && x < 24.5f && z > -44.5 && z < -34.5)
        {
            return true;
        }

        return false;
    }
#endregion
}
