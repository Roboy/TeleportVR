using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WheelchairStateManager : Singleton<WheelchairStateManager>
{
    [SerializeField] private GameObject[] WheelchairModels;
    [SerializeField] private GameObject UpperBody, Legs;
    public const float HUDAlpha = .4f;

    public bool visible
    {
        get { return _visible; }
        set
        {
            _visible = value;
            SetVisibility(_visible);
        }
    }
    private bool _visible = true;
    private StateManager.States internalState;

    private void Start()
    {
        internalState = StateManager.Instance.currentState;
    }

    void Update()
    {
        // if state changed update visibility accordingly
        if (StateManager.Instance.currentState != internalState)
        {
            internalState = StateManager.Instance.currentState;
            SetVisibility(internalState != StateManager.States.HUD);
        }
    }

    /// <summary>
    /// shows / hides the classes game objects
    /// </summary>
    /// <param name="show">if true objs are shown, otherwise hidden</param>
    /// <param name="alpha">optional transpaceny</param>
    public void SetVisibility(bool show, float alpha = 1)
    {
        Debug.Log($"set visibility {show}, {alpha}");
        _visible = show;
        List<GameObject> models = new List<GameObject>(WheelchairModels);
        foreach (var model in models)
        {
            if (model != null)
            {
                model.SetActive(show);
            }
        }
        models.Add(UpperBody);
        models.Add(Legs);
        // if BioIK is needed for real roboy, only the meshes might need to be disabled, but for now just disable it all
        foreach (var obj in models)
        {
            if (obj == null)
            {
                continue;
            }
            foreach (var r in obj.GetComponentsInChildren<Renderer>())
            {
                r.enabled = show;
                if (alpha < 1)
                {
                    MaterialExtensions.ToFadeMode(r.material);
                }
                else
                {
                    MaterialExtensions.ToOpaqueMode(r.material);
                }
                Color color = r.material.color;
                color.a = alpha;
                r.material.color = color;
            }
        }
    }
}
