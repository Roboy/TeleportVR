using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    public int resWidth = 3840;
    public int resHeight = 2160;

    private bool takeHiResShot = false;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}x{2}_{3}.png",
            Application.dataPath,
            width, height,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    /// <summary>
    /// Take a screenshot when pressing 'v'.
    /// </summary>
    void LateUpdate()
    {
        takeHiResShot |= Input.GetKeyDown("v");
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
            GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);

            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            Application.OpenURL(filename);
            takeHiResShot = false;
        }
    }
}