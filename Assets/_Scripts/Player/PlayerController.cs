using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimController animController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Stackable playerStackable;
    [SerializeField] private InputActionReference movement;
    [SerializeField] private InputActionReference punchAction;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int maxStack = 1;

    private Vector3 prevInput;
    private Camera cameraReference;

    private void Awake()
    {
        cameraReference = Camera.main;

        if (playerStackable == null)
        {
            playerStackable = this.GetComponent<Stackable>();

            if (playerStackable == null)
            {
                Debug.LogError("Could not find player's Stackable Component");
            }
        }
    }

    private void OnEnable()
    {
        punchAction.action.started += TryPunch;
    }

    private void OnDisable()
    {
        punchAction.action.started -= TryPunch;
    }

    public void IncreaseStackSize(int increment)
    {
        maxStack += increment;
    }

    void TryPunch(InputAction.CallbackContext context)
    {
        int stackedCount = 0;

        Stackable iterate = playerStackable;

        while (iterate.GetNext() != null)
        {
            stackedCount++;
            iterate = iterate.GetNext();
        }

        if (stackedCount >= maxStack)
        {
            return;
        }

        animController.RequestPunch();
    }

    private void UpdateMovement(Vector3 movement)
    {
        animController.SetBlendValue(movement.magnitude);

        if (movement.magnitude < 0.1f)
        {
            playerStackable.ApplyVelocity(transform.forward * 0.001f);
        }
        else
        {
            float angle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg
                + cameraReference.transform.rotation.eulerAngles.y;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

            Vector3 displacement = transform.forward * speed;

            characterController.Move(displacement * Time.deltaTime);

            playerStackable.ApplyVelocity(displacement);
        }

        playerStackable.AdjustStackable();
    }

    private void Update()
    {
        Vector2 rawInput = movement.action.ReadValue<Vector2>();
        Vector3 newInput = new Vector3(rawInput.x, 0.0f, rawInput.y);

        // Smoothing the end of the movement input
        Vector3 currentInput = Vector3.Lerp(prevInput, newInput, Time.deltaTime * 10.0f);
        UpdateMovement(currentInput);
        prevInput = currentInput;
    }
}
