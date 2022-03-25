using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentManager : MonoBehaviour
{
    public float trainingDuration = 30;
    public List<Agent> agents = new List<Agent>();

    public int populationSize = 100;

    public Agent agentPrefab;
    public Transform agentGroup;
    public Agent agent;

    public float mutationRate = 5;

    public TMP_Text timerText;
    public float startingTime;

    public CameraController cameraController;

    [SerializeField] TMP_Text numberGeneration;
    public int generationCount = 0;

    public ProceduralManager pcManager;
    public bool firstLoop = false;

    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        NewGeneration();
        ResetTimer();
        Focus();
        yield return new WaitForSeconds(trainingDuration);
        StartCoroutine(Loop());
    }

    private void FixedUpdate()
    {
        agents.Sort();
        CarNumber();
        Focus();
    }

    void NewGeneration()
    {
        AddOrRemoveAgents();
        Mutate();
        ResetAgents();
        SetMaterials();
        GenerationCount();
        if (firstLoop)
        {
            generateTerrain();
        }
        firstLoop = true;
    }

    void GenerationCount()
    {
        numberGeneration.text = generationCount++.ToString();
    }

    void CarNumber()
    {
        for (int i = 0; i < agents.Count; i++)
        {
           
            if (i < 5)
            {
                agents[i].carPlacement.text = (i + 1).ToString();
            }
            else
            {
                agents[i].carPlacement.text = "";
            }
        }
    }


    void AddOrRemoveAgents()
    {
        if (agents.Count != populationSize)
        {
            int dif = populationSize - agents.Count;

            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();
                }
            }
            else
            {
                for (int i = 0; i < -dif; i++)
                {
                    RemoveAgent();
                }
            }
        }
    }

    void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].gameObject);
        agents.RemoveAt(agents.Count - 1);
    }

    void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup);
        agent.net = new NeuralNetwork(agentPrefab.net.layers);

        agents.Add(agent);
    }

    void Mutate()
    {
        for (int i = agents.Count/5; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i - agents.Count/5].net);
            agents[i].net.Mutate(mutationRate);

            agents[i].SetMutatedMaterial();
        }

    }
    void ResetAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].ResetAgent();
        }
    }


    void SetMaterials()
    {
        for (int i = 1; i < agents.Count / 5; i++)
        {
            agents[i].SetDefaultMaterial();
        }

        agents[0].SetFirstMaterial();
    }
    void ResetTimer()
    {
        startingTime = Time.time;
    }

    void Update()
    {
        RefreshTimer();
    }

    void RefreshTimer()
    {
        timerText.text = (trainingDuration - (Time.time - startingTime)).ToString("f0");

    }

    void Focus()
    {
        NeuralNetworkViewer.instance.RefreshAgent(agents[0]);
        cameraController.target = agents[0].transform;
    }

    public void Refocus()
    {
        agents.Sort();
        Focus();
    }

    public void Save()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }
        Data data = new Data {nets = nets, gen = generationCount };

        DataManager.instance.Save(data);
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if (data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }

            generationCount = data.gen;
        }

        End();
    }

    public void End()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());
    }

    void generateTerrain()
    {
        pcManager.InstantiatePrefab();

        if (pcManager.tilesUsed.Count == pcManager.numberOfTilesToGenerate)
        {
            Debug.Log("coucou");
            pcManager.generationToWait--;
            pcManager.DestroyTiles();
        }
    }
}
