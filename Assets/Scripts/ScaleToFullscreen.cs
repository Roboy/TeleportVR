using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToFullscreen : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Camera camera;
     
    void OnGUI()
    {
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float distance = transform.position.z - camera.transform.position.z;
        float screenHeight = 2 * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * distance;
        float screenWidth = screenHeight * camera.aspect;
        transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteWidth, 1f);
    }
}
