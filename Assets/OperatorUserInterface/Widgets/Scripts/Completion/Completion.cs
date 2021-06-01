using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Widgets
{
    public class Completion : MonoBehaviour
    {
        public string text = "Calibrating";
        [Range(0f, 1f)]
        public float progress = 0;
        public bool active = false;
        public Image image;
        public GameObject child;

        private TextMeshProUGUI tmp;

        // Start is called before the first frame update
        void Start()
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            progress = Mathf.Max(0, Mathf.Min(progress, 1));
            active &= progress > 0;
            child.SetActive(active);
            image.fillAmount = progress;

            tmp.text = text;
        }
    }

}
