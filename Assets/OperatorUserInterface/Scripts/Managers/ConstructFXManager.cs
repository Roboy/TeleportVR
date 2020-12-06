using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using Valve.VR.InteractionSystem;

public class ConstructFXManager : MonoBehaviour {
    [SerializeField] private Material constructEffectMaterial;

    private GameObject[] constructEffectObjects;

    private GameObject[] constructEffectObjectsArt;

    /// <summary>
    /// Start. Gets necessary game objects.
    /// </summary>
    void Start() {
        constructEffectObjects = GameObject.FindGameObjectsWithTag("ConstructFX");
        constructEffectObjectsArt = GameObject.FindGameObjectsWithTag("ConstructFXArt");
    }

    /// <summary>
    /// Toggles the gallery room between normal and construct mode.
    /// Enables (Disables) futuristic material on the walls, floor and other stuff in the room.
    /// </summary>
    /// <param name="effectActive">decides, whether the effect should be active afterwards</param>
    public void ToggleEffects(bool effectActive) {
        foreach (var constructEffectObject in constructEffectObjects) {
            MeshRenderer meshRenderer = constructEffectObject.GetComponent<MeshRenderer>();

            Material prevMaterial = meshRenderer.materials[0];


            if (effectActive) {
                Material[] tmpMaterials = new Material[meshRenderer.materials.Length + 1];
                for (int i = 0; i < meshRenderer.materials.Length; i++) {
                    tmpMaterials[i] = meshRenderer.materials[i];
                }

                tmpMaterials[meshRenderer.materials.Length] = constructEffectMaterial;

                meshRenderer.materials = tmpMaterials;
            }
            else {
                Material[] tmpMaterials = new Material[meshRenderer.materials.Length - 1];
                for (int i = 0; i < meshRenderer.materials.Length - 1; i++) {
                    tmpMaterials[i] = meshRenderer.materials[i];
                }

                meshRenderer.materials = tmpMaterials;
            }
        }

        foreach (var constructEffectObject in constructEffectObjectsArt) {
            constructEffectObject.SetActive(!constructEffectObject.activeSelf);
        }
    }
}