using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] CarController carController;
    void Update()
    {
        carController.horizontalInput = Input.GetAxis("Horizontal");
        carController.verticalInput = Input.GetAxis("Vertical");
    }
}
