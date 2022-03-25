using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralManager : MonoBehaviour
{
    public GameObject[] tilesToGenerate;
    public int numberOfTilesToGenerate;
    public int MutationNumberForFirstLevel;
    public AgentManager agentManager;
    public int generationToWait;
    int u;
    public int w;

    public List<Transform> transformSpawnPoints = new List<Transform>();
    public List<GameObject> tileGenerated = new List<GameObject>();
    public List<Transform> tilesUsed = new List<Transform>();

    public int numberOfTilesGeneratePerTurn;
    private void Start()
    {
        u = generationToWait;
    }
    public void InstantiatePrefab()
    {   
        if (MutationNumberForFirstLevel <= agentManager.generationCount && transformSpawnPoints.Count > 0)
        {
            if (w > 0)
            {
                w--;
            }
            if (tileGenerated.Count < numberOfTilesToGenerate && w ==0)
            {
                for (int i = 0; i < numberOfTilesGeneratePerTurn; i++)
                {
                    int random = Random.Range(0, transformSpawnPoints.Count);
                    GameObject tg = Instantiate(tilesToGenerate[Random.Range(0, tilesToGenerate.Length)], transformSpawnPoints[random].position, Quaternion.identity);
                    tileGenerated.Add(tg);
                    tilesUsed.Add(transformSpawnPoints[random]);
                    transformSpawnPoints.Remove(transformSpawnPoints[random]);

                    if (w == 0 && transformSpawnPoints.Count > 0)
                    {
                        w = generationToWait;
                    }

                }
            }
        }
    }

    public void DestroyTiles()
    {
        if (generationToWait == 0)
        {
            generationToWait = u;
            Debug.Log("2");
            GameObject tile = tileGenerated[Random.Range(0, tileGenerated.Count)];
            GameObject newTile = Instantiate(tilesToGenerate[Random.Range(0, tilesToGenerate.Length)], tile.transform.position, Quaternion.identity);
            Destroy(tile);
            tileGenerated.Remove(tile);
            tileGenerated.Add(newTile);
        }
    }
}