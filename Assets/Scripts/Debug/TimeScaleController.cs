using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float timescale = 1;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timescale;
    }
}
