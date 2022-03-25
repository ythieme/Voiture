using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Agent : MonoBehaviour, IComparable<Agent>
{
    public NeuralNetwork net;

    public CarController carController;

    public float fitness;
    [SerializeField] Rigidbody rb;
    float totalCheckpointDist;

    public float[] inputs;
    public Transform nextCheckpoint;
    public float nextCheckpointDist;

    public LayerMask layerMask;
    public LayerMask layerToAvoid;
    public float rayRange = 5f;
    public float obstacleRayRange = 5f;
    public float malus = 0f;
    public bool grounded;


    public TMP_Text carPlacement;


    public void ResetAgent()
    {
        fitness = 0f;
        malus = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        totalCheckpointDist = 0f;

        inputs = new float[net.layers[0]];

        carController.Reset();

        nextCheckpoint = CheckpointManager.instance.firstCheckpoint;
        nextCheckpointDist = (nextCheckpoint.position - transform.position).magnitude;
    }

    void FixedUpdate()
    {
        InputUpdate();
        OutputUpdate();
        UpdateFitness();
    }

    void InputUpdate()
    {
        //raycast verifiant l'environnement
        inputs[0] = RaySensor(transform.position, transform.forward, 4f, layerMask); 
        inputs[1] = RaySensor(transform.position, transform.right, 1.5f, layerMask); 
        inputs[2] = RaySensor(transform.position, -transform.right, 1.5f, layerMask); 
        inputs[3] = RaySensor(transform.position, transform.forward + transform.right , 2f, layerMask); 
        inputs[4] = RaySensor(transform.position, transform.forward - transform.right , 2f, layerMask);

        //Vitesse du v�hicule
        /*inputs[5] = (float)Math.Tanh(rb.velocity.magnitude * 0.05f);
        inputs[6] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);*/

        //Permet de garder le syst�me en marche si
        //rien n'est detect� et que le v�hicule ne bouge pas
        inputs[5] = 1f;
        inputs[6] = CollisionSensor(transform.position, transform.forward, 6f, layerToAvoid, QueryTriggerInteraction.UseGlobal); 
        inputs[7] = CollisionSensor(transform.position, transform.forward + transform.right, 4f, layerToAvoid, QueryTriggerInteraction.UseGlobal); 
        inputs[8] = CollisionSensor(transform.position, transform.forward - transform.right, 4f, layerToAvoid, QueryTriggerInteraction.UseGlobal); 
        inputs[9] = CollisionSensor(transform.position,transform.right, 4f, layerToAvoid, QueryTriggerInteraction.UseGlobal); 
        inputs[10] = CollisionSensor(transform.position,- transform.right, 4f, layerToAvoid, QueryTriggerInteraction.UseGlobal); 
    }

    RaycastHit hit;
    float RaySensor(Vector3 origin, Vector3 dir, float length, LayerMask layerMask)
    {
        if (Physics.Raycast(origin, dir, out hit, length * rayRange, layerMask))
        {
            Debug.DrawRay(origin, dir * hit.distance, 
                Color.Lerp(Color.blue, Color.black, 1 - hit.distance / (length * rayRange)));
            return 1 - (hit.distance / (length * rayRange));
        }
        else
        {
            Debug.DrawRay(origin, dir * length * rayRange, Color.blue);
            return 0f;
        }
    }
    float CollisionSensor(Vector3 origin, Vector3 dir, float length, LayerMask layerMask, QueryTriggerInteraction triggerInteraction)
    {

        if (Physics.Raycast(origin, dir, out hit, length * obstacleRayRange, layerMask))
        {
            triggerInteraction = QueryTriggerInteraction.Collide;
            Debug.DrawRay(origin, dir * hit.distance,
                Color.Lerp(Color.red, Color.green, 3 - hit.distance / (length * obstacleRayRange)));

                return 1 - (hit.distance / (length * obstacleRayRange));
        }
        else
        {
            Debug.DrawRay(origin, dir * length * obstacleRayRange, Color.red);
            return 0f;
        }
    }

    private void OutputUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length -1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    public float distanceTraveled;
    public void CheckpointReached(Transform checkpoint)
    {
        totalCheckpointDist += nextCheckpointDist;
        nextCheckpoint = checkpoint;

        nextCheckpointDist = (nextCheckpoint.position - transform.position).magnitude;
    } 
    void UpdateFitness()
    {
        distanceTraveled = (totalCheckpointDist + nextCheckpointDist - (nextCheckpoint.position - transform.position).magnitude);

        if (fitness < (distanceTraveled) - malus)
        {
            fitness = distanceTraveled - malus;
        }
        else if (fitness > (distanceTraveled) - malus)
        {
            fitness = distanceTraveled - malus;
        }
    }

    public MeshRenderer render;
    public MeshRenderer mapRender;
    public Material firstMat;
    public Material mutatedMat;
    public Material defaultMat;

    public void SetFirstMaterial()
    {
        render.material = firstMat;
        mapRender.material = firstMat;
    }
    public void SetMutatedMaterial()
    {
        render.material = mutatedMat;
        mapRender.material = mutatedMat;
    }

    public void SetDefaultMaterial()
    {
        render.material = defaultMat;
        mapRender.material = defaultMat;
    }

    public int CompareTo(Agent other)
    {
        if (fitness < other.fitness)
        {
            return 1;
        }

        if (fitness > other.fitness)
        {
            return -1;
        }

        return 0;
    }
}
