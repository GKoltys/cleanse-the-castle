using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private bool hasBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (!hasBounds) return;

        // camera half-sizes
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        // camera follows player
        Vector3 desired = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        // camera view stays inside dungeon
        desired.x = Mathf.Clamp(
            desired.x,
            minX + halfWidth,
            maxX - halfWidth
        );

        desired.y = Mathf.Clamp(
            desired.y,
            minY + halfHeight,
            maxY - halfHeight
        );

        // Safety check if map is smaller than camera view
        float clampMinX = minX + halfWidth;
        float clampMaxX = maxX - halfWidth;
        float clampMinY = minY + halfHeight;
        float clampMaxY = maxY - halfHeight;

        if (clampMinX > clampMaxX) desired.x = (minX + maxX) * 0.5f;
        else desired.x = Mathf.Clamp(desired.x, clampMinX, clampMaxX);

        if (clampMinY > clampMaxY) desired.y = (minY + maxY) * 0.5f;
        else desired.y = Mathf.Clamp(desired.y, clampMinY, clampMaxY);

        transform.position = desired;
    }

    public void SetBounds(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        hasBounds = true;
    }
}
