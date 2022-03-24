using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 localPositionToMove = new Vector3(0, 5, -15);
    public Vector3 localPositionToLook = new Vector3(0, -1, 5);

    public float movingSpeed = 0.02f;
    public float rotationSpeed = 0.1f;

    Vector3 wantedPosition;
    Quaternion wantedRotation;

    void Update()
    {
        wantedPosition = target.TransformPoint(localPositionToMove);
        wantedPosition.y = target.position.y + localPositionToMove.y;

        transform.position = Vector3.Lerp(transform.position, wantedPosition, movingSpeed);

        wantedRotation = Quaternion.LookRotation(target.TransformPoint(localPositionToLook) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, rotationSpeed);
    }
}
