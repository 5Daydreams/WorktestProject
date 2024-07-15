using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Punchable : MonoBehaviour
{
    [SerializeField] private GameObject rootObject;
    [SerializeField] private Stackable myStackable;
    [SerializeField] private ParticleSystem defeatVfx;
    [SerializeField] private float bouncynessOnHit;
    private List<Rigidbody> ragdollList;

    private void Awake()
    {
        ragdollList = this.GetComponentsInChildren<Rigidbody>().ToList();

        foreach (var body in ragdollList)
        {
            body.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Punch"))
        {
            Vector3 direction = (this.transform.position - other.transform.position).normalized;

            ragdollList[0].AddForce(direction * bouncynessOnHit, ForceMode.Impulse);

            Transform iterate = other.transform;

            // find the topmost parent of the player prefab, where the player's stackable component is
            while (iterate.parent != null)
            {
                iterate = iterate.transform.parent.transform;
            }

            Stackable playerStackable = iterate.gameObject.GetComponent<Stackable>();

            // I can't tell if this is necessary or not, but I prefer having the extra line here over having the instantiation within the next line
            Stackable myInstance = Instantiate(myStackable);

            playerStackable.SetNext(myInstance);

            DestroyBehavior();
        }
    }


    private void DestroyBehavior()
    {
        foreach (var body in ragdollList)
        {
            body.isKinematic = false;
        }

        defeatVfx.Play();

        Destroy(rootObject, defeatVfx.main.duration);
    }
}
