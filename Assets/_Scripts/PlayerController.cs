using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimController animController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference movement;
    [SerializeField] private float speed = 3.0f;
    private Vector3 prevInput;
    private Camera cameraReference;

    private void Awake()
    {
        cameraReference = Camera.main;
    }

    private void UpdateMovement(Vector3 movement)
    {
        animController.SetBlendValue(movement.magnitude);

        Vector3 tempDirection = (transform.position - cameraReference.transform.position);
        tempDirection.y = 0;

        Quaternion motionRotation = Quaternion.LookRotation(tempDirection);

        Vector3 adjustedMovement = motionRotation * movement;
        adjustedMovement.y = 0.0f;

        characterController.SimpleMove(adjustedMovement * speed);

        if (movement.magnitude < 0.1f)
        {
            return;
        }

        Vector3 newRotationVector = (transform.position - cameraReference.transform.position).normalized + adjustedMovement;
        newRotationVector.y = 0;
        newRotationVector.Normalize();

        Quaternion gameObjectRotation = Quaternion.LookRotation(newRotationVector);

        this.transform.rotation = gameObjectRotation;
    }

    private void Update()
    {
        Vector2 rawInput = movement.action.ReadValue<Vector2>();
        Vector3 newInput = new Vector3(rawInput.x, 0.0f, rawInput.y);

        // Smoothing the end of the movement input
        Vector3 currentInput = Vector3.Lerp(prevInput, newInput, Time.deltaTime * 8.0f);
        UpdateMovement(currentInput);
        prevInput = currentInput;
    }
}
