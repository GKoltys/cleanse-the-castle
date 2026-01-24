using UnityEngine;
using System.Collections;

public class StairsController : MonoBehaviour, IMapGenInit
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