using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class SubMenuAnimationHandler : MonoBehaviour
{
    struct InteractionPrefab
    {
        public string TagName;
        public UnityAction SafeModeOnAction;
        public UnityAction SafeModeOffAction;
        public List<GameObject> FoundObjects;

        public InteractionPrefab(string tag, UnityAction on, UnityAction off) {
            TagName = tag;
            SafeModeOnAction = on;
            SafeModeOffAction = off;
            FoundObjects = new List<GameObject>();
        }
    }

    public bool MButtonTransition;
    //Set this to true, if there are UI elements which are not direct children of the InteractionObjects object
    public bool IsNested = false;
    public bool HookToOpenMenuButton = false;
    public bool AcivateOnFadeOut;
    public GameObject ElementToActivate;
    private int currentState;
    private bool newRequest;
    private bool fadeIn;
    private FrameClickDetection openMenuButton;

    List<InteractionPrefab> interactionPrefabs;

    private Animator animator;

    private void OnDestroy()
    {
        if (HookToOpenMenuButton)
        {
            openMenuButton.onPress[0].RemoveListener(FadeIn);
        }
    }

    /// <summary>
    /// Set reference to instances.
    /// Find all UI element instances within the descendants of InteractionObjects
    /// </summary>
    void Start()
    {
        // Dirty Engineering for final Demo, necessary because SenseGloves not in same Scene
        // Unfortunately no 
        if(AcivateOnFadeOut && ElementToActivate == null)
        {
            ElementToActivate = GameObject.FindGameObjectWithTag("SenseGloveLeft").transform.GetChild(1).gameObject;
        }
        if (HookToOpenMenuButton)
        {
            openMenuButton = GameObject.FindGameObjectWithTag("SenseGloveLeft").transform.GetChild(1).GetChild(1).GetComponent<FrameClickDetection>();
            if(openMenuButton != null)
            {
                openMenuButton.onPress[0].AddListener(new UnityAction(FadeIn));
            } else
            {
                Debug.LogError("SenseGloveLeft not found during Start.");
                HookToOpenMenuButton = false;
            }
            
        }

        currentState = -1;
        newRequest = false;
        fadeIn = true;
        animator = transform.parent.GetComponent<Animator>();
        interactionPrefabs = new List<InteractionPrefab>();

        interactionPrefabs.Add(new InteractionPrefab("Button3D", safemodeOnButton3D, safemodeOffButton3D));
        interactionPrefabs.Add(new InteractionPrefab("Slider3D", safemodeOnSlider3D, safemodeOffSlider3D));

        if (!IsNested)
        {
            foreach (InteractionPrefab prefab in interactionPrefabs)
            {
                List<GameObject> currentList = new List<GameObject>();
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).CompareTag(prefab.TagName))
                        {
                            currentList.Add(transform.GetChild(i).gameObject);
                        }
                    }
                prefab.FoundObjects.AddRange(currentList);
            }
        }
        else
        {
            foreach (InteractionPrefab prefab in interactionPrefabs)
            {
                    prefab.FoundObjects.AddRange(findObjectsWithTagInAllChildren(prefab.TagName, transform));
            }
        }

        safeModeOn();
        animator.SetTrigger("Go");
    }

    /// <summary>
    /// Execute menu animations.
    /// </summary>
    private void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (currentState != state.fullPathHash)
        {
            currentState = state.fullPathHash;
            if (state.IsName("Visible"))
            {
                safeModeOff();
                animator.SetBool("FadeIn", false);
            } else if(state.IsName("Invisible")) {
                deActivate(false);
                animator.SetBool("FadeOut", false);
            }
        }

        if (newRequest && (state.IsName("Visible") || state.IsName("Invisible")))
        {
            newRequest = false;
            if (fadeIn && state.IsName("Invisible"))
            {
                deActivate(true);
                animator.SetBool("FadeIn", true);
            }
            else if(!fadeIn && state.IsName("Visible")) {
                safeModeOn();
                animator.SetBool("FadeOut", true);
            } 
        }

        if(MButtonTransition && Input.GetKeyDown(KeyCode.M))
        {
            if (fadeIn)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }
    }

    /// <summary>
    /// Find all objects of a certain tag among the descendants of the parent object
    /// </summary>
    /// <param name="tag">The tag</param>
    /// <param name="parent">The parent object, which descendants are checked</param>
    /// <returns></returns>
    List<GameObject> findObjectsWithTagInAllChildren(string tag, Transform parent)
    {
        List<GameObject> list = new List<GameObject>();
        Transform currentChild;
        for (int i = 0; i < transform.childCount; i++)
        {
            currentChild = transform.GetChild(i);
            if (currentChild.CompareTag(tag))
            {
                list.Add(currentChild.gameObject);
            }
            list.AddRange(findObjectsWithTagInAllChildren(tag, currentChild));
        }
        return list;
    }

    /// <summary>
    /// Enable/Disable menu
    /// </summary>
    /// <param name="activate">Activate, false: deactivate</param>
    private void deActivate(bool activate)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(activate);
        }

        int myIndex = transform.GetSiblingIndex();
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (i != myIndex)
            {
                parent.GetChild(i).gameObject.SetActive(activate);
            }
        }
    }

    /// <summary>
    /// Public method to initiate fade in of the menu.
    /// </summary>
    public void FadeIn()
    {
        newRequest = true;
        fadeIn = true;
    }
    /// <summary>
    /// Public method to initiate fade out of the menu.
    /// If ActivateOnFadeOut it true, the ElementToActivate is enabled.
    /// </summary>
    public void FadeOut()
    {
        newRequest = true;
        fadeIn = false;
        if (AcivateOnFadeOut)
        {
            Assert.IsNotNull(ElementToActivate, "If you set 'ActivateOnFadeOut' to true, then you must specify the 'ElementToActivate'.");
            ElementToActivate.SetActive(true);
            if(ElementToActivate.CompareTag("Button3D"))
            {
                ElementToActivate.transform.GetChild(0).GetComponent<ButtonRigidbodyConstraint>().InitialState();
                ElementToActivate.transform.GetChild(1).GetComponent<FrameClickDetection>().highlightOff();
            }
        }
    }

    /// <summary>
    /// Activates the safe mode for all interaction objects, so that they can be modified by the animation.
    /// </summary>
    private void safeModeOn()
    {
        foreach (InteractionPrefab prefab in interactionPrefabs)
        {
            prefab.SafeModeOnAction.Invoke();
        }
    }
    /// <summary>
    /// Deactivate the safe mode for all interaction objects, so that they are interactable again.
    /// </summary>
    private void safeModeOff()
    {
        foreach (InteractionPrefab prefab in interactionPrefabs)
        {
            prefab.SafeModeOffAction.Invoke();
        }
    }

    #region safe mode methods for each interactionPrefab
    /// <summary>
    /// Put 3D Button in safe mode by disabling collider and the FrameClickDetection script.
    /// </summary>
    private void safemodeOnButton3D()
    {
        List<GameObject> allButtons = interactionPrefabs.Find(x => x.TagName.Equals("Button3D")).FoundObjects;
        foreach (GameObject obj in allButtons)
        {
            Transform frame = obj.transform.GetChild(1);
            frame.GetComponent<FrameClickDetection>().highlightOff();
            frame.GetComponent<Collider>().enabled = false;
            frame.GetComponent<FrameClickDetection>().enabled = false;

            Transform activeArea = obj.transform.GetChild(2);
            activeArea.GetComponent<Collider>().enabled = false;
        }
    }
    /// <summary>
    /// Revert 3D button safe mode changes by enabling collider and the FrameClickDetection script.
    /// </summary>
    private void safemodeOffButton3D()
    {
        List<GameObject> allButtons = interactionPrefabs.Find(x => x.TagName.Equals("Button3D")).FoundObjects;
        foreach (GameObject obj in allButtons)
        {
            Transform frame = obj.transform.GetChild(1);
            frame.GetComponent<Collider>().enabled = true;
            frame.GetComponent<FrameClickDetection>().enabled = true;
        }
    }

    /// <summary>
    /// Put 3D slider in safe mode by disabling collider and the CustomSlider script.
    /// </summary>
    private void safemodeOnSlider3D()
    {
        List<GameObject> allSliders = interactionPrefabs.Find(x => x.TagName.Equals("Slider3D")).FoundObjects;
        foreach (GameObject obj in allSliders)
        {
            obj.transform.GetChild(0).GetComponent<Collider>().enabled = false;
            obj.transform.GetChild(0).GetComponent<CustomSlider>().enabled = false;
        }
    }
    /// <summary>
    /// Revert 3D slider safe mode changes by enabling collider and the CustomSlider script.
    /// </summary>
    private void safemodeOffSlider3D()
    {
        List<GameObject> allSliders = interactionPrefabs.Find(x => x.TagName.Equals("Slider3D")).FoundObjects;
        foreach (GameObject obj in allSliders)
        {
            Transform full = obj.transform.GetChild(0);
            CustomSlider customslider = full.GetComponent<CustomSlider>();
            customslider.ReturnToDefaultPos();
            full.GetComponent<Collider>().enabled = true;
            customslider.enabled = true;
        }
    }
    #endregion
}