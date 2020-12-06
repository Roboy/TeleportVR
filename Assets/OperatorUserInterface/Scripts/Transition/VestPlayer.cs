using System.Collections;
using System.Collections.Generic;
using Bhaptics.Tact.Unity;
using UnityEngine;

public class VestPlayer : MonoBehaviour
{
    [SerializeField] [Tooltip("All tactsources that shall be played in order")]
    public TactSource[] Sources;
    
    /// <summary>
    /// Starts coroutine to play tact sources which are given by the user in the inspector.
    /// </summary>
    public void playTact()
    {
        StartCoroutine(playTactSources());
    }

    /// <summary>
    /// Plays all given tact sources with a 0.025 second gap.
    /// </summary>
    /// <returns>WaitForSeconds in order to wait 0.025 seconds.</returns>
    IEnumerator playTactSources()
    {
        foreach (var tactSource in Sources)
        {
            tactSource.Play();
            yield return new WaitForSeconds(0.025f);
        }
    }
}
