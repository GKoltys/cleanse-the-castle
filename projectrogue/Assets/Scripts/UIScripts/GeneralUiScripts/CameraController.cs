using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

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

        transform.position = desired;
    }

    public void SetBounds(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }
}
