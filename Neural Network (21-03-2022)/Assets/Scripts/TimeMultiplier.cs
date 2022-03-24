using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMultiplier : MonoBehaviour
{
    public Slider timeSlider;
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeSlider.value;
    }
}
