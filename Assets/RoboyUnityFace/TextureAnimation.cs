using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimation : MonoBehaviour {

    public float speed;
    public GameObject waterTile;
    public bool moving = false;

    private Vector3 startPosition;
    private int timer;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (moving)
        {
            if ((timer % 70) == 0)
            {
                var go = Instantiate(waterTile, startPosition, Quaternion.identity);
                go.transform.parent = transform;
                transform.position = startPosition;
            }


            foreach (Transform child in transform)
            {
                //Check if texture is still visible
                if (child.position.y < (startPosition.y - 30))
                {
                    Destroy(child.gameObject);
                }

                //Move the tiles down
               Vector3 newPosition = new Vector3(child.position.x, child.position.y - speed, child.position.z);
               child.position = newPosition;

            }
            timer++;
            Debug.Log(timer);

        }
	}
}
