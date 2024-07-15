using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEmissionGlow : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private MeshRenderer myRenderer;
    [Tooltip("Must be 0-1 duration")]
    [SerializeField] private AnimationCurve sampleCurve;
    [SerializeField] private bool useCurve;
    [SerializeField] private float sineDelay;
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();

        myRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        mpb.Clear();

        float curve = 0.0f;

        if (useCurve)
        {
            float sampleValue = Time.time % 1.0f;
            curve = sampleCurve.Evaluate(sampleValue);
        }
        else
        {
            curve = Mathf.Sin(Time.time * Mathf.PI / sineDelay) * 0.5f + 0.5f;
        }

        Color result = gradient.Evaluate(curve);

        //mpb.SetColor("_Color", result);
        mpb.SetColor("_EmissionColor", result);

        myRenderer.SetPropertyBlock(mpb);
    }
}
