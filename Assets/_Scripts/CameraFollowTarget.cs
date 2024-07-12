using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private Vector3 lookOffset = Vector3.zero;
    [SerializeField] private float rotationLerpSpeed = 1.0f;

    [Header("Follow")]
    [SerializeField] private float followDistance = 1.0f;
    [SerializeField] private float cameraHeight = 1.0f;
    [SerializeField] private float followLerpSpeed = 1.0f;

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("The 'target' was not found - please assign a value in the inspector for " + gameObject.name);
            this.enabled = false;
        }
    }

    void FollowBehavior()
    {
        Vector3 currentPosition = transform.position;

        Vector3 desiredPosition = transform.position - target.position;
        desiredPosition.y = 0.0f;
        desiredPosition.Normalize();

        float horizontalDistance = Mathf.Sqrt((followDistance * followDistance) - (cameraHeight * cameraHeight));
        desiredPosition *= horizontalDistance;

        desiredPosition.y = cameraHeight;

        transform.position = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * followLerpSpeed);
    }

    void RotationBehavior()
    {
        // Projecting OffsetValues in relation to target position
        Vector3 offset = target.right * lookOffset.x + target.up * lookOffset.y + target.forward * lookOffset.z;

        Vector3 targetFwd = (target.position + offset - transform.position).normalized;
        Vector3 currentFwd = transform.forward;

        Vector3 newFwd = Vector3.Lerp(currentFwd, targetFwd, Time.deltaTime * rotationLerpSpeed);

        Quaternion newRot = Quaternion.LookRotation(newFwd, Vector3.up);

        transform.rotation = newRot;
    }

    void LateUpdate()
    {
        FollowBehavior();
        RotationBehavior();
    }
}
