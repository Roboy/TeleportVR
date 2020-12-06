using UnityEngine;

/// <summary>
/// Helper class for a passive timer. Timer does not count down time itself, but has to be called manually. 
/// </summary>
public class Timer 
{
    private float duration;
    public float timer;

    public delegate void TimeIsUp();
    private TimeIsUp timeIsUp;    

    /// <summary>
    /// Called when timer should count down passed time.
    /// </summary>
    /// <param name="deltaTime">Amount of time the timer should count down</param>
    public void LetTimePass(float deltaTime)
    {
        timer += deltaTime;

        if (timer > duration)
        {
            timeIsUp();
        }
    }

    /// <summary>
    /// Returns, how much percent of the timer has passed in [0,1].
    /// </summary>
    /// <returns></returns>
    public float GetFraction()
    {
        return (timer / duration);
    }

    /// <summary>
    /// Set duration and callback of timer.
    /// </summary>
    /// <param name="duration">Duration of timer</param>
    /// <param name="timeIsUp">Callback function called when time is up</param>
    public void SetTimer(float duration, TimeIsUp timeIsUp)
    {
        this.duration = duration;
        timer = 0;
        this.timeIsUp = timeIsUp;
    }

    /// <summary>
    /// Reset timer to zero.
    /// </summary>
    public void ResetTimer()
    {
        timer = 0;
    }
}
