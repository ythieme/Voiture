using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int malusValue;
    [SerializeField] TMP_Text passedText;
    int carPassed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Agent>())
        {
            other.GetComponent<Agent>().malus += malusValue;
            carPassed += 1;
            passedText.text = carPassed.ToString();
        }
    }
}
