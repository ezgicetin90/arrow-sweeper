using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform anchorTarget;
    public float followSpeed = 5f;
    public float yOffset = 5f;
    public float zOffset = -6f;

    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = true;

    private void Start()
    {
        AdjustOrthographicSize();
    }

    public void SetTarget(Transform newTarget)
    {
        anchorTarget = newTarget;
        isFollowing = (newTarget != null);
    }

    void LateUpdate()
    {
        if (!isFollowing || anchorTarget == null) return;

        Vector3 desiredPos = new Vector3(
            anchorTarget.position.x,
            anchorTarget.position.y + yOffset,
            anchorTarget.position.z + zOffset
        );

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, 1f / followSpeed);
    }

    private void AdjustOrthographicSize()
    {
        Camera cam = GetComponent<Camera>();
        if (cam == null || !cam.orthographic) return;

        float referenceAspect = 9f / 16f; // Target aspect ratio
        float currentAspect = (float)Screen.width / Screen.height;
        float scaleFactor = currentAspect / referenceAspect;

        if (scaleFactor < 1f)
        {
            cam.orthographicSize = cam.orthographicSize / scaleFactor;
        }
    }
}