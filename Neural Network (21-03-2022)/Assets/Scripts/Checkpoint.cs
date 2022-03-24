using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform nextCheckPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Agent>())
        {
            if (other.transform.GetComponent<Agent>().nextCheckpoint == transform)
            {
                other.transform.GetComponent<Agent>().CheckpointReached(nextCheckPoint);
            }
        }
    }
}
