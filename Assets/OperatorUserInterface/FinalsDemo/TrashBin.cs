using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    /// <summary>
    /// Checks if a trash cube has been released at the opening of the trashbin.
    /// If it is, the trash cube is placed at a random position and in a random rotation within the trashbin.
    /// </summary>
    /// <param name="other">The collider of the object that intersects with the opening of the trashbin</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Trash") && !other.GetComponent<SenseGlove_Grabable>().IsInteracting())
        {
            other.transform.SetParent(this.transform);
            other.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.4f, 0.07f), Random.Range(-0.14f, 0.1f));
            other.transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
    }
}
