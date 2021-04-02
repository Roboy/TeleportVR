using System.Collections;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WheelChairControl : MonoBehaviour
{
    //public float Forward { get; set; }
    //public float Backward { get; set; }
    //public float Right { get; set; }
    //public float Left { get; set; }

    //public IEnumerator GetDataFromFirebase()
    //{
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://wheelchair-control-unity.firebaseio.com/.json");
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log(www.downloadHandler.text);
    //        var controller = ReadToObject(www.downloadHandler.text);
    //        Debug.Log(controller.Forward);

    //        Forward = controller.Forward;
    //        Backward = controller.Backward;
    //        Right = controller.Right;
    //        Left = controller.Left;

    //    }
    //}

    //// Deserialize a JSON stream to a User object.  
    //public static WheelChairControl ReadToObject(string json)
    //{
    //    var deserializedUser = new WheelChairControl();
    //    var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
    //    var ser = new DataContractJsonSerializer(deserializedUser.GetType());
    //    deserializedUser = ser.ReadObject(ms) as WheelChairControl;
    //    ms.Close();
    //    return deserializedUser;
    //}
}
