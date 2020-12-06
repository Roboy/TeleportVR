using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    /*
     * Deprecated at the moment
     * The idea is to improve the collision detection of the finger tip with the pressure plate of a 3D Button.
     * For that an additional collider is added where its properties are tweaked to give the best experience
     * for the button interaction (the collider settings can still be improved: main problem is that the finger
     * sometimes pushes through the pressure plate, probably no collision detected)
     * When a sense glove enters the ActiveArea (encapsulating the button) the custom collider is enabled and the
     * regular sense glove collider is disabled.
     * Vice versa when exiting the ActiveArea.
     */

    /*private Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = this.GetComponents<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("ActiveArea"))
        {
            Debug.Log(this.name + " TriggerEnter detected");
            colliders[0].enabled = false;
            colliders[1].enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("ActiveArea"))
        {
            Debug.Log(this.name + " TriggerExit detected");
            colliders[0].enabled = true;
            colliders[1].enabled = false;
        }
    }*/
}
