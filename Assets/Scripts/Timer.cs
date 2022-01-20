using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    static private System.Random rand = new System.Random();
    public float GetRandomFloat(float min = -1, float max = 1)
    {
        if (rand == null) rand = new System.Random();
        var f = rand.Next(System.Convert.ToInt32(min * 100), System.Convert.ToInt32(max * 100));
        float result = f / 100.0f;
        return result;
    }
    public int GetRandomInteger(int min = 0, int max = 10)
    {
        return rand.Next(min, max);
    }
}
