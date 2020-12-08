using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTag : MonoBehaviour
{
    /// <summary>
    /// Changes the name tag of this object
    /// </summary>
    /// <param name="name">New Name</param>
    public void updateTag(string name)
    {
        this.GetComponent<TextMeshPro>().text = name;
    }
}
