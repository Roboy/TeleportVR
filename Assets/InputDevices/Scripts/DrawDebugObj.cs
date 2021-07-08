using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JointTransfer
{
    public class DrawDebugObj : MonoBehaviour
    {

        public float scale = 0.01f;
        public PrimitiveType primitiveType = PrimitiveType.Cube;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var child in Utils.GetChildren(transform, g => g.name.Contains("_")))
            {
                var cube = GameObject.CreatePrimitive(primitiveType);
                cube.transform.parent = child.transform;
                cube.transform.localPosition = Vector3.zero;
                cube.transform.localRotation = Quaternion.identity;
                cube.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}
