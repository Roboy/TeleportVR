using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This stores a set of SenseGlove Poses over a number of time steps 
/// and allows computations on them.
/// </summary>
public class PoseBuffer
{
    private Vector3[][] poses;
    private int index;
    private readonly int bufferSize;
    private bool clean;

    public PoseBuffer(int bufferSize = 30)
    {
        this.poses = new Vector3[bufferSize][];
        index = 0;
        this.bufferSize = bufferSize;
        clean = true;
    }

    public void AddPose(Vector3[] pose)
    {
        clean = false;
        poses[index] = pose;
        index = Mod((index + 1), bufferSize);
    }


    public void Clear()
    {
        if (!clean)
        {
            this.poses = new Vector3[bufferSize][];
            this.index = 0;
        }
        clean = true;
    }


    /// <summary>
    /// Computes the MSE over all time steps in the buffer
    /// </summary>
    /// <returns>MSE</returns>
    public float ComputeError()
    {
        Vector3 acc = Vector3.zero;
        // loop backwards in time (t) through the buffer
        int ticks = 0;
        for (int t = Mod(index - 1, bufferSize); t != index; t = Mod(t - 1, bufferSize))
        {
            int prev = Mod(t - 1, bufferSize);
            if (poses[t] == null || poses[prev] == null) break;
            ticks++;

            for (int f = 0; f < poses[t].Length; f++)
            {
                acc += Vector3.Scale(poses[t][f] - poses[prev][f], poses[t][f] - poses[prev][f]);
            }
        }
        return ticks > 0 ? (acc.x + acc.y + acc.z) / (3 * ticks) : 0;
    }

    private int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
