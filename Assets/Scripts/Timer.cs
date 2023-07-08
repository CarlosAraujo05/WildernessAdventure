using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float time = 0f;
    void FixedUpdate()
    {
        time += Time.deltaTime;
    }
}
