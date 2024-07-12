using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimController animController;
    [SerializeField] private CharacterController characterController;

    public void InputHandler(Vector2 input)
    {
        Vector3 param = new Vector3(input.x, input.y);
    }

    private void UpdateMovement(Vector3 movement)
    {
        characterController.SimpleMove(movement);
        animController.SetBlendValue(movement.magnitude);
    }
}
