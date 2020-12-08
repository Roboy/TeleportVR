using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskbarManager : MonoBehaviour
{
    [SerializeField]
    private List<Thumbnail> thumbnailsSownOnTaskbar = 
        new List<Thumbnail> {Thumbnail.Settings, Thumbnail.Clock, Thumbnail.Battery, Thumbnail.Template};

    public IconStateManager[] Thumbnails;

    private void Awake()
    {
        Thumbnails = GetComponentsInChildren<IconStateManager>();
    }

    public void StartParty()
    {
        foreach (IconStateManager stateManager in Thumbnails)
        {
            stateManager.ShowNotification();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}

public enum Thumbnail
{
    Battery, Settings, Clock, Template
}
