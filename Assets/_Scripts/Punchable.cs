using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punchable : MonoBehaviour
{
    [SerializeField] private Stackable myStackable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Punch")
        {
            Debug.Log("Collision happened");

            // Stack this on top of the player

            Transform iterate = other.transform;

            while (iterate.parent != null)
            {
                iterate = iterate.transform.parent.transform;
            }

            Stackable playerStackable = iterate.gameObject.GetComponent<Stackable>();

            Stackable myInstance = Instantiate(myStackable);

            playerStackable.SetNext(myInstance);

            this.enabled = false;

            StartCoroutine(WaitToDestroy());

        }
    }


    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.050f);
        Destroy(this.gameObject);
    }
}
