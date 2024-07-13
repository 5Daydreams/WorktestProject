using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour
{
    [SerializeField] private Vector3 nextGap;
    [SerializeField] private float lerpSpeed;
    public Stackable next;


    private void Update()
    {
        if(next == null)
        {
            return;
        }

        Vector3 positionLerp = Vector3.Lerp(next.transform.position, transform.position + nextGap, Time.deltaTime * lerpSpeed);
    }
}
