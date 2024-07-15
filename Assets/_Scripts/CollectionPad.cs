using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]

public class CollectionPad : MonoBehaviour
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

            if (player.GetNext() != null)
            {
                player.RemoveTopmost();
                deliverEvent.Invoke();
            }
        }
    }
}
