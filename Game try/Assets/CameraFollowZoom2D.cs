using UnityEngine;

public class CameraFollowZoom2D : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    [Header("Follow Settings")]
    public Vector3 offset;
    public float smoothTime = 0.2f;

    [Header("Zoom Settings")]
    public float minZoom = 5f;
    public float maxZoom = 12f;
    public float zoomLimiter = 10f;

    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (!player1 || !player2) return;

        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 targetPosition = centerPoint + offset;

        // Lock Z
        targetPosition.z = -10f;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }



    void ZoomCamera()
    {
        // Get distance between players
        float distanceX = Mathf.Abs(player1.position.x - player2.position.x);
        float distanceY = Mathf.Abs(player1.position.y - player2.position.y);

        // Camera aspect ratio
        float aspect = cam.aspect;

        // Calculate required size for BOTH axes
        float sizeBasedOnHeight = distanceY / 2f;
        float sizeBasedOnWidth = (distanceX / 2f) / aspect;

        // Take the larger value so both players fit
        float requiredSize = Mathf.Max(sizeBasedOnHeight, sizeBasedOnWidth);

        // Add padding so players aren't at screen edges
        requiredSize += 2f;

        // Clamp zoom
        float clampedSize = Mathf.Clamp(requiredSize, minZoom, maxZoom);

        // Smooth zoom
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            clampedSize,
            Time.deltaTime * 5f
        );
    }



    Vector3 GetCenterPoint()
    {
        return (player1.position + player2.position) / 2f;
    }

}
