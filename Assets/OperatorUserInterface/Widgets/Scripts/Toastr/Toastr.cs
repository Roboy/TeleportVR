using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{   
    public class Toastr : MonoBehaviour
    {
        public string msg;
        public Color color;
        public int fontSize;

        private readonly float SLERP_DURATION = 0.5f;

        private bool slerpActive = false;
        private Vector3 localSlerpStartPos;
        private Vector3 localSlerpStopPos;
        private Timer timer;

        TextMeshProUGUI textMeshPro;

        /// <summary>
        /// Initializes toastr.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        /// <param name="fontSize"></param>
        public void Init(string msg, Color color, int fontSize)
        {
            this.msg = msg;
            this.color = color;
            this.fontSize = fontSize;

            textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.SetText(msg);
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color;

            timer = new Timer();

            gameObject.AddComponent<CurvedUI.CurvedUIVertexEffect>();
        }

        /// <summary>
        /// Move the toastr upwards over time by a slerp. Called when top toastr is deleted. 
        /// For nicer animation, a time offset is used depending on the position of the toastr to delay animation for lower toastr.
        /// </summary>
        /// <param name="offsetInY">How far to slerp upwards</param>
        /// <param name="timeOffset">Delay slerp for this toastr by offset.</param>
        public void SlerpUp(float offsetInY, float timeOffset)
        {
            localSlerpStartPos = transform.localPosition;
            localSlerpStopPos = transform.localPosition + new Vector3(0, offsetInY, 0);
            timer.SetTimer(SLERP_DURATION + timeOffset, StopSlerp);
            slerpActive = true;
        }

        /// <summary>
        /// Manage timer.
        /// </summary>
        public void Update()
        {
            if (slerpActive)
            {
                timer.LetTimePass(Time.deltaTime);

                transform.localPosition = Vector3.Slerp(localSlerpStartPos, localSlerpStopPos, timer.GetFraction());
            }
        }

        /// <summary>
        /// Set slerp flag false.
        /// </summary>
        public void StopSlerp()
        {
            slerpActive = false;
        }
    }
}