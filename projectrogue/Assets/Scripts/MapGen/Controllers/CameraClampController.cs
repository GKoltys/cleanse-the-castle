using UnityEngine;
using UnityEngine.Tilemaps;

public class StartingAreaCameraClamp : MonoBehaviour
{
    [SerializeField] private CameraController cam;
    [SerializeField] private TilemapCollider2D tmc;

    // Clamp camera using TilemapCollider2D bounds
    void Start()
    {
        Vector3 tmcCenter = tmc.bounds.center;
        Vector3 tmcExtent = tmc.bounds.extents;

        cam.SetBounds(tmcCenter.x - tmcExtent.x, tmcCenter.x + tmcExtent.x, tmcCenter.y - tmcExtent.y, tmcCenter.y + tmcExtent.y);
    }

    // Manually clamp camera when needed using params from MapData (assuming bottom left is (0, 0))
    public void SetBoundsAfterGeneration(int width, int height)
    {
        cam.SetBounds(0, width, 0, height);
    }
}
