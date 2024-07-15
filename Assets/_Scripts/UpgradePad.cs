using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradePad : MonoBehaviour
{
    [SerializeField] private float deliveryCooldown = 1.0f;
    [SerializeField] private UnityEvent deliverEvent;
    private Stackable player;

    private float timer = 0.0f;
    private bool playerInside;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.GetComponent<Stackable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }

    private void Update()
    {
        if (!playerInside)
        {
            timer = 0.0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer > deliveryCooldown)
        {
            timer = 0.0f;

            deliverEvent.Invoke();
        }
    }
}
