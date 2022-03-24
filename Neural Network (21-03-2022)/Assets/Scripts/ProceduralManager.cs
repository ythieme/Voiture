using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralManager : MonoBehaviour
{
    public Transform[] transformSpawnPoint;
    public GameObject[] tilesToGenerate;
    public int numberOfTilesToGenerate;
    public int MutationNumberForFirstLevel;
    public AgentManager agentManager;

    public List<GameObject> tileGenerated = new List<GameObject>();

    public int numberOfTilesGeneratePerTurn;
    public void InstantiatePrefab()
    {   
        if (MutationNumberForFirstLevel <= agentManager.generationCount)
        {
            Debug.Log("0");
            if (tileGenerated.Count < numberOfTilesToGenerate)
            {

                for (int i = 0; i < numberOfTilesGeneratePerTurn; i++)
                {
                    int random = Random.Range(0, transformSpawnPoint.Length);
                    GameObject tg = Instantiate(tilesToGenerate[Random.Range(0, tilesToGenerate.Length)], transformSpawnPoint[random].position, Quaternion.identity);
                    tileGenerated.Add(tg);
                }
            }
        }
    }

    public void DestroyTiles()
    {
        Debug.Log("2");
       
        for (int i = 0; i < tileGenerated.Count; i++)
        {
            GameObject tile = tileGenerated[Random.Range(0, tileGenerated.Count)];
            tileGenerated.Remove(tile);
            Debug.Log("3");
            Destroy(tile);
        }
    }
}