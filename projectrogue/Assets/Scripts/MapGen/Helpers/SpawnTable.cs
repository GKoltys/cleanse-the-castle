using UnityEngine;

[System.Serializable]
public class SpawnTable {

    public string id;

    public GameObject prefab;

    [Min(0)]
    public int count = 1;

    [Min(0)]
    public float minDistanceFromPlayer = 0f;

    public bool uniqueTile = true;
}