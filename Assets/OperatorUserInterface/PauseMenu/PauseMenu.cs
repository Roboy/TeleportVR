using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{
    [Range(0, 1)]
    public bool show;
    public GameObject child;

    [Header("UI Buttons")]
    public TouchButton switchScene;
    public bool switchScenePressed = false;


    // Start is called before the first frame update
    void Start()
    {
        // recover values presence detector when this script is reloaded
        show = RudderPedals.PresenceDetector.Instance.isPaused;
        switchScenePressed = RudderPedals.PresenceDetector.Instance.isPaused;

        // buttons init
        switchScene.OnTouchEnter(() =>
        {
            if (switchScenePressed) return;

            switchScenePressed = true;
            switch (StateManager.Instance.currentState)
            {
                case StateManager.States.Training:
                    Debug.Log("Changing scene to HUD");
                    StateManager.Instance.GoToState(StateManager.States.HUD);
                    break;
                case StateManager.States.HUD:
                    Debug.Log("Changing scene to Traning");
                    StateManager.Instance.GoToState(StateManager.States.Training);
                    break;
            }
        });
        switchScene.OnTouchExit(() =>
        {
            switchScenePressed = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        switch (StateManager.Instance.currentState)
        {
            case StateManager.States.Training:
                switchScene.text = "Control";
                break;
            case StateManager.States.HUD:
                switchScene.text = "Training";
                break;
        }
        child.SetActive(show);
    }

    private void OnDestroy()
    {
        switchScene.ClearOnTouchEnter();
        switchScene.ClearOnTouchExit();
    }
}
