using System.Collections;
using System.Collections.Generic;
#if BHAPTICS
using Bhaptics.Tact.Unity;
#endif
using UnityEngine;

public class VestPlayer : MonoBehaviour
{
#if BHAPTICS
    [SerializeField] [Tooltip("All tactsources that shall be played in order")]
    public TactSource[] Sources;
#endif

    /// <summary>
    /// Starts coroutine to play tact sources which are given by the user in the inspector.
    /// </summary>
    public void playTact()
    {
#if BHAPTICS
        StartCoroutine(playTactSources());
#endif
    }

    /// <summary>
    /// Plays all given tact sources with a 0.025 second gap.
    /// </summary>
    /// <returns>WaitForSeconds in order to wait 0.025 seconds.</returns>
    IEnumerator playTactSources()
    {
#if BHAPTICS
        foreach (var tactSource in Sources)
        {
            tactSource.Play();
        yield return new WaitForSeconds(0.025f);
        }
#else
        yield return new WaitForSeconds(0.025f);
#endif
}
}
