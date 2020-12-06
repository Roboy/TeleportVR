using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SubMenuScaling : MonoBehaviour
{
#if UNITY_EDITOR

    #region properties
    public float width;
    public float height;
    private float oldWidth;
    private float oldHeight;

    public string horizontalAlignmentText;
    public string verticalAlignmentText;
    private int horizontalAlignment;
    private int verticalAlignment;
    
    private Transform topCorners;
    private Transform bottomCorners;
    private Transform topLeftCorner;
    private Transform topRightCorner;
    private Transform bottomLeftCorner;
    private Transform bottomRightCorner;
    private Transform verticalSides;
    private Transform horizontalSides;
    private Transform leftSide;
    private Transform rightSide;
    private Transform topSide;
    private Transform bottomSide;
    private Transform nameTag;
    #endregion

    /// <summary>
    /// Initial setup
    /// </summary>
    private void Start()
    {
        runInEditMode = true;
        findChildren();
    }

    /// <summary>
    /// Reset the menu to its original scale
    /// </summary>
    private void Reset()
    {
        Vector3 zeroVector = new Vector3(0, 0, 0);
        Vector3 oneVector = new Vector3(1, 1, 1);

        width = 1;
        height = 1;

        horizontalAlignment = 0;
        verticalAlignment = 1;
        horizontalAlignmentText = "center";
        verticalAlignmentText = "top";

        findChildren();

        topCorners.localPosition = zeroVector;
        topLeftCorner.localPosition = zeroVector;
        topRightCorner.localPosition = zeroVector;
        bottomCorners.localPosition = zeroVector;
        bottomLeftCorner.localPosition = zeroVector;
        bottomRightCorner.localPosition = zeroVector;

        verticalSides.localScale = oneVector;
        horizontalSides.localScale = oneVector;
        leftSide.localPosition = new Vector3(-0.0697f, 0, 0);
        rightSide.localPosition = new Vector3(0.0697f, 0, 0);
        topSide.localPosition = new Vector3(0, 0.1005f, 0);
        bottomSide.localPosition = new Vector3(0, -0.1005f, 0);

        nameTag.localPosition = new Vector3(0.6229f, 0.1237f, 0);
    }
    
    /// <summary>
    /// Setting references to all frame components for quick access.
    /// </summary>
    private void findChildren()
    {
        topCorners = this.transform.GetChild(0);
        topLeftCorner = topCorners.GetChild(0);
        topRightCorner = topCorners.GetChild(1);

        bottomCorners = this.transform.GetChild(1);
        bottomLeftCorner = bottomCorners.GetChild(0);
        bottomRightCorner = bottomCorners.GetChild(1);

        verticalSides = this.transform.GetChild(2).GetChild(0);
        leftSide = verticalSides.GetChild(0);
        rightSide = verticalSides.GetChild(1);

        horizontalSides = this.transform.GetChild(2).GetChild(1);
        topSide = horizontalSides.GetChild(0);
        bottomSide = horizontalSides.GetChild(1);

        nameTag = this.transform.parent.GetChild(1);

        oldWidth = width;
        oldHeight = height;
    }

    /// <summary>
    /// Update is called once per frame during playmode.
    /// Update is called on every scene change during editmode.
    /// 
    /// Managing the scaling of the Menu during editmode.
    /// </summary>
    void Update()
    {
        if (Application.isPlaying)
        {
            this.enabled = false;
        }
        else
        {
            if (oldWidth != width)
            {
                //lossyScale should work fine. If it returns incorrect results, check whether children's rotaion cause skew.
                //Then try: oldLength = topSide.localScale.y * horizontalSides.localScale.x * adjustableSides.localScale.x * ...;
                float oldLength = topSide.lossyScale.y;
                float newLength = topSide.localScale.y * width;
                float changeValue = (newLength - oldLength) / 2;

                leftSide.localPosition = new Vector3(leftSide.localPosition.x - changeValue, leftSide.localPosition.y, leftSide.localPosition.z);
                rightSide.localPosition = new Vector3(rightSide.localPosition.x + changeValue, rightSide.localPosition.y, rightSide.localPosition.z);
                topLeftCorner.localPosition = new Vector3(topLeftCorner.localPosition.x - changeValue, topLeftCorner.localPosition.y, topLeftCorner.localPosition.z);
                topRightCorner.localPosition = new Vector3(topRightCorner.localPosition.x + changeValue, topRightCorner.localPosition.y, topRightCorner.localPosition.z);
                bottomLeftCorner.localPosition = new Vector3(bottomLeftCorner.localPosition.x - changeValue, bottomLeftCorner.localPosition.y, bottomLeftCorner.localPosition.z);
                bottomRightCorner.localPosition = new Vector3(bottomRightCorner.localPosition.x + changeValue, bottomRightCorner.localPosition.y, bottomRightCorner.localPosition.z);
                nameTag.localPosition = new Vector3(nameTag.localPosition.x + (changeValue * horizontalAlignment), nameTag.localPosition.y, nameTag.localPosition.z);

                horizontalSides.localScale = new Vector3(width, horizontalSides.localScale.y, horizontalSides.localScale.z);
                oldWidth = width;
            }


            if (oldHeight != height)
            {
                //lossyScale should work fine. If it returns incorrect results, check whether children's rotaion cause skew.
                //Then try: oldLength = leftSide.localScale.y * verticalSides.localScale.y * adjustableSides.localScale.y * ...;
                float oldLength = leftSide.lossyScale.y;
                float newLength = leftSide.localScale.y * height;
                float changeValue = (newLength - oldLength) / 2;

                topSide.localPosition = new Vector3(topSide.localPosition.x, topSide.localPosition.y + changeValue, topSide.localPosition.z);
                bottomSide.localPosition = new Vector3(bottomSide.localPosition.x, bottomSide.localPosition.y - changeValue, bottomSide.localPosition.z);
                topCorners.localPosition = new Vector3(topCorners.localPosition.x, topCorners.localPosition.y + changeValue, topCorners.localPosition.z);
                bottomCorners.localPosition = new Vector3(bottomCorners.localPosition.x, bottomCorners.localPosition.y - changeValue, bottomCorners.localPosition.z);
                nameTag.localPosition = new Vector3(nameTag.localPosition.x, nameTag.localPosition.y + (changeValue * verticalAlignment), nameTag.localPosition.z);

                verticalSides.localScale = new Vector3(verticalSides.localScale.x, height, verticalSides.localScale.z);
                oldHeight = height;
            }
        }
    }

    #region button logic: nameTag alignment
    public void setHorizontalCenter()
    {
        horizontalAlignmentText = "center";
        horizontalAlignment = 0;
    }

    public void setHorizontalLeft()
    {
        horizontalAlignmentText = "left";
        horizontalAlignment = -1;
    }

    public void setHorizontalRight()
    {
        horizontalAlignmentText = "right";
        horizontalAlignment = 1;
    }

    public void setVerticalCenter()
    {
        verticalAlignmentText = "center";
        verticalAlignment = 0;
    }

    public void setVerticalTop()
    {
        verticalAlignmentText = "top";
        verticalAlignment = 1;
    }

    public void setVerticalBottom()
    {
        verticalAlignmentText = "bottom";
        verticalAlignment = -1;
    }
    #endregion

#endif
}
