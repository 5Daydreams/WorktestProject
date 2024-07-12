using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string blendName;
    [SerializeField] private string triggerName;
    private float blendValue;

    public void SetBlendValue(float value)
    {
        blendValue = value;
    }

    private void Awake()
    {
        if (animator == null)
        {
            this.animator = GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError("Could not find animator in game object " + gameObject.name + " even after using GetComponent<T>(). Please recheck Component values");
            }
        }
    }


    void Update()
    {
        animator.SetFloat(blendName, blendValue);
    }

    [ContextMenu("RequestPunchDebug")]
    void RequestPunch()
    {
        animator.SetTrigger(triggerName);
    }
}
