using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes {
    NONE,
    HUD,
    CONSTRUCT
}

public class AdditiveSceneManager : MonoBehaviour {
    static Scenes currentScene = Scenes.NONE;

    public delegate void BeforeSceneLoadDelegate();

    public delegate void OnSceneLoadDelegate();

    public delegate void BeforeSceneUnloadDelegate();

    public delegate void OnSceneUnloadDelegate();

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Returns the currently loaded scene
    /// </summary>
    /// <returns>the currently loaded scene</returns>
    public static Scenes GetCurrentScene() {
        return currentScene;
    }

    /// <summary>
    /// Delegate that executes everytime a scene is loaded to update currentScene variable.
    /// </summary>
    /// <param name="scene">laoded scene</param>
    /// <param name="mode">mode used to load the sene</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch (scene.name) {
            case "FinalScene_Construct":
                currentScene = Scenes.CONSTRUCT;
                break;
            case "FinalScene_HUD":
                currentScene = Scenes.HUD;
                break;
        }
    }

    /// <summary>
    /// Finds scene name for scene
    /// </summary>
    /// <param name="scene">scene</param>
    /// <returns>name of scene</returns>
    String SceneNameForScene(Scenes scene) {
        switch (scene) {
            case Scenes.HUD:
                return "FinalScene_HUD";
            case Scenes.CONSTRUCT:
                return "FinalScene_Construct";
            default:
                return "";
        }
    }

    /// <summary>
    /// Performs a scene change. First Unloads a scene, then Load the next Scene
    /// </summary>
    /// <param name="newScene">the new scene that shall be loaded</param>
    /// <param name="beforeSceneUnload">delegate that is executed before the current scene is unloaded</param>
    /// <param name="onSceneUnload">delegate that ist executed after the current scene is unloaded</param>
    /// <param name="beforeSceneLoad">delegate that is executed before the new scene is loaded</param>
    /// <param name="onSceneLoad">delegate that is executed after the new scene is loaded</param>
    public void ChangeScene(Scenes newScene, BeforeSceneUnloadDelegate beforeSceneUnload, OnSceneUnloadDelegate onSceneUnload, BeforeSceneLoadDelegate beforeSceneLoad, OnSceneLoadDelegate onSceneLoad) {
        StartCoroutine(UnloadScene(newScene, beforeSceneUnload, onSceneUnload, beforeSceneLoad, onSceneLoad));
    }

    /// <summary>
    /// Loads an additive scene. 
    /// 
    /// Additive scene can be either [Scenes.CONSTRUCT] or [Scenes.HUD].
    /// </summary>
    /// <param name="beforeSceneUnload">Handler executed before scene is loaded. If null, default handler is executed.</param>
    /// <param name="onSceneUnload">Handler executed after scene is loaded. If null, default handler is executed.</param>
    IEnumerator LoadScene(Scenes scene, BeforeSceneLoadDelegate beforeSceneLoad, OnSceneLoadDelegate onSceneLoad) {
        if (currentScene != Scenes.NONE) {
            throw new Exception("An error happened. There is another scene loaded. This can't be.");
        }

        String sceneName = SceneNameForScene(scene);
        if (sceneName != "") {
            if (beforeSceneLoad != null) {
                beforeSceneLoad();
            }
            else {
                DefaultBeforeSceneLoad();
            }

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (asyncOperation != null) {
                asyncOperation.completed += delegate(AsyncOperation op) {
                    currentScene = scene;

                    if (onSceneLoad != null) {
                        onSceneLoad();
                    }
                    else {
                        DefaultOnSceneLoad();
                    }
                };
            }
        }

        yield break;
    }

    /// <summary>
    /// Unloads an additive scene. 
    /// 
    /// Additive scene can be either [Scenes.CONSTRUCT] or [Scenes.HUD].
    /// </summary>
    /// <param name="beforeSceneUnload">Handler executed before scene is unloaded. If null, default handler is executed.</param>
    /// <param name="onSceneUnload">Handler executed after scene is unloaded. If null, default handler is executed.</param>
    IEnumerator UnloadScene(Scenes newScene, BeforeSceneUnloadDelegate beforeSceneUnload, OnSceneUnloadDelegate onSceneUnload, BeforeSceneLoadDelegate beforeSceneLoad, OnSceneLoadDelegate onSceneLoad) {
        if (currentScene == Scenes.NONE) {
            // Load new scene, as no scene is currently loaded
            StartCoroutine(LoadScene(newScene, beforeSceneLoad, onSceneLoad));
            yield break;
        }

        String sceneName = SceneNameForScene(currentScene);
        if (sceneName != "") {
            if (beforeSceneUnload != null) {
                beforeSceneUnload();
            }
            else {
                DefaultBeforeSceneUnload();
            }

            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            if (asyncOperation != null) {
                asyncOperation.completed += delegate(AsyncOperation op) {
                    currentScene = Scenes.NONE;

                    if (onSceneUnload != null) {
                        onSceneUnload();
                    }
                    else {
                        DefaultOnSceneUnload();
                    }

                    // Load new scene, after current scene has been unloaded
                    StartCoroutine(LoadScene(newScene, beforeSceneLoad, onSceneLoad));
                };
            }
        }
    }

    /// <summary>
    /// Default handler executed before scene is loaded.
    /// </summary>
    private void DefaultBeforeSceneLoad() {
    }

    /// <summary>
    /// Default handler executed after scene is loaded.
    /// </summary>
    private void DefaultOnSceneLoad() {
    }

    /// <summary>
    /// Default handler executed before scene is unloaded.
    /// </summary>
    private void DefaultBeforeSceneUnload() {
    }

    /// <summary>
    /// Default handler executed after scene is unloaded.
    /// </summary>
    private void DefaultOnSceneUnload() {
    }

    /// <summary>
    /// Demo method to show how delegates work.
    /// </summary>
    private void DelegateDemo() {
        Debug.Log("Delegate!");
    }
}