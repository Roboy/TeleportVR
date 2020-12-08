using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToParentSize : MonoBehaviour
{
    //Should default to true
    public bool HasFixedScale;
    RectTransform parent;
    Vector2 size;

    /// <summary>
    /// Set reference to parent.
    /// Initial adaptation of this collider to the scale of the parent.
    /// </summary>
    void Start()
    {
        parent = this.transform.parent.GetComponent<RectTransform>();
        Rescale();
    }

    /// <summary>
    /// If the parent's scale changes adapt accordingly.
    /// </summary>
    void FixedUpdate()
    {
        if (!HasFixedScale)
        {
            if(parent.sizeDelta != size)
            {
                Rescale();
            }
        }
    }

    /// <summary>
    /// Adapt scale of this collider to the scale of the parent.
    /// </summary>
    private void Rescale()
    {
        size = parent.sizeDelta;
        this.transform.localScale = new Vector3(size.x, size.y, this.transform.localScale.z);
    }
}
