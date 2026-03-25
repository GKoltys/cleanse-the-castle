using UnityEngine;

public class FogFollow : MonoBehaviour
{
    [SerializeField] private Transform cam;

    private void LateUpdate()
    {
        transform.position = new Vector3(cam.position.x, cam.position.y, 0);
    }
}