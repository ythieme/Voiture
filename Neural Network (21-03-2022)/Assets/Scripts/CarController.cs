using System;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] float maxSteerAngle = 42;
    [SerializeField] float motorForce = 800;

    [SerializeField] Transform wheelFrontLeft, wheelFrontRight, wheelRearLeft, wheelRearRight;
    [SerializeField] WheelCollider wheelColliderFrontLeft, wheelColliderFrontRight,wheelColliderRearLeft, wheelColliderRearRight;

    //output du machine learning ou input du joueur
    public float horizontalInput;
    public float verticalInput;

    [SerializeField] Transform centerOfMass;
    [SerializeField] Rigidbody rb;
    void Start()
    {
        rb.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    void Accelerate()
    {
        wheelColliderRearLeft.motorTorque = verticalInput * motorForce;
        wheelColliderRearRight.motorTorque = verticalInput * motorForce;

       // wheelColliderFrontLeft.motorTorque = verticalInput * motorForce;
       // wheelColliderFrontRight.motorTorque = verticalInput * motorForce;
    }

    void Steer()
    {
        wheelColliderFrontLeft.steerAngle = horizontalInput * maxSteerAngle;
        wheelColliderFrontRight.steerAngle = horizontalInput * maxSteerAngle;
    }
    void UpdateWheelPoses()
    {
        UpdateWheelPose(wheelColliderFrontLeft, wheelFrontLeft);
        UpdateWheelPose(wheelColliderFrontRight, wheelFrontRight);
        UpdateWheelPose(wheelColliderRearLeft, wheelRearLeft);
        UpdateWheelPose(wheelColliderRearRight, wheelRearRight);
    }

    Vector3 pos;
    Quaternion quat;
    void UpdateWheelPose(WheelCollider col, Transform tr)
    {
        pos = tr.position;
        quat = tr.rotation;

        col.GetWorldPose(out pos, out quat);

        tr.position = pos;
        tr.rotation = quat;
    }

    public void Reset()
    {
        horizontalInput = 0;
        verticalInput = 0;
    }
}
