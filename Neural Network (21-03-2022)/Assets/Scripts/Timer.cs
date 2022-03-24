using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeValue;
    public TMP_Text timertxt;
    // Update is called once per frame
    void Update()
    {
        timeValue += Time.deltaTime;

        timertxt.text = (Mathf.RoundToInt(timeValue) / 60 ).ToString("00") 
            + ":" + (timeValue % 60f).ToString("00");
    }
}
