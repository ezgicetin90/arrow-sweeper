using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float stopFollowHeight = 0f;
    private bool isFollowing = true;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    void LateUpdate()
    {
        if (!isFollowing || target == null) return;

        float cameraY = transform.position.y;
        float targetY = target.position.y;

        // Stop when camera reaches stopFollowHeight
        if (targetY <= stopFollowHeight)
        {
            isFollowing = false;
            return;
        }

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(cameraY, targetY, followSpeed * Time.deltaTime);
        transform.position = pos;
    }
}