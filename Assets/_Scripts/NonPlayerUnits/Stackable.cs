using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour
{
    [SerializeField] private float stackDistance = 1.0f;
    [SerializeField] private float wobbleIntensity = 1.0f;
    [SerializeField] private ParticleSystem removalVfx;
    [SerializeField] private GameObject meshObject;
    [HideInInspector] public Vector3 velocity;

    private Vector3 currentForward = Vector3.forward;
    private Vector3 currentfakeRight = Vector3.right;
    private float wobbleAngle = 0.0f;
    private Stackable next;
    private GameObject fakeGO;

    private void Awake()
    {
        fakeGO = new GameObject();
        fakeGO.transform.parent = transform;
    }

    public Stackable GetNext()
    {
        return next;
    }

    public void SetNext(Stackable nextValue)
    {
        nextValue.wobbleIntensity += 0.2f;
        if (next == null)
        {
            next = nextValue;
        }
        else
        {
            next.SetNext(nextValue);
        }
    }

    public void ApplyVelocity(Vector3 newVelocity)
    {
        velocity = Vector3.Lerp(velocity, newVelocity, Time.deltaTime * 5.0f);
        //velocity = newVelocity;

        float speed = velocity.magnitude;

        if (speed > 0.5f)
        {
            wobbleAngle = wobbleIntensity * speed * speed;
        }
        else
        {
            wobbleAngle = Mathf.Lerp(wobbleAngle, 0.0f, Time.deltaTime * wobbleIntensity);
        }

        currentForward = Vector3.Lerp(currentForward, velocity, Time.deltaTime * 2.0f);
    }

    public void AdjustStackable()
    {
        if (next == null)
        {
            return;
        }

        Transform fakeTransform = fakeGO.transform;

        fakeTransform.position = this.transform.position;
        fakeTransform.rotation = Quaternion.identity;

        fakeTransform.position += this.transform.up * stackDistance;

        Vector3 fakeRight = Vector3.Cross(Vector3.up, currentForward);

        currentfakeRight = Vector3.Lerp(currentfakeRight, fakeRight, Time.deltaTime * wobbleAngle).normalized;

        fakeTransform.RotateAround(this.transform.position + transform.up * stackDistance * 0.75f, currentfakeRight, -wobbleAngle);

        next.transform.position = fakeTransform.position;
        next.transform.rotation = fakeTransform.rotation;

        next.ApplyVelocity(this.velocity * 1.1f);
        next.AdjustStackable();
    }

    // Recursion is required to check each element of the stack
    public void RemoveTopmost()
    {
        bool isPlayer = this.CompareTag("Player");
        bool lastElement = next == null;

        if (lastElement)
        {
            if (isPlayer)
            {
                return;
            }

            DestroyBehavior();
        }
        else
        {
            next.RemoveTopmost();
        }
    }

    private void DestroyBehavior()
    {
        meshObject.SetActive(false);
        removalVfx.Play();

        Destroy(gameObject, removalVfx.main.duration);
    }
}
