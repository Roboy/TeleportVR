using Tobii.G2OM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EyeGazeDwellTimer : MonoBehaviour, IGazeFocusable, IPointerEnterHandler, IPointerExitHandler
{
    public float duration = 2.0f;
    Timer timer;
    public bool hasFocus = false;

    Image dwellTimeImage;

    private void Awake()
    {
        timer = new Timer();
        dwellTimeImage = GetComponentInParent<Image>();
    }

    public void GazeFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            OnPointerEnter(null);
        }
        else
        {
            OnPointerExit(null);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFocus)
        {
            timer.LetTimePass(Time.deltaTime);
            dwellTimeImage.fillAmount = timer.GetFraction();
        }
    }

    public void GoToConstruct()
    {
        StateManager.Instance.GoToNextState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hasFocus = true;
        timer.SetTimer(duration, GoToConstruct);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasFocus = false;

        timer.ResetTimer();
        dwellTimeImage.fillAmount = timer.GetFraction();
    }
}
