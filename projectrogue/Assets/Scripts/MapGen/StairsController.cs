using UnityEngine;

public class StairsController : MonoBehaviour
{
    private MapGenerator dungeon;

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (other.CompareTag("Player"))
        {
            dungeon.GoToNextFloor();
        }
    }
}
