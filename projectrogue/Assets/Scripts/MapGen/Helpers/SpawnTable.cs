using UnityEngine;

public enum SpawnType
{
    Any,        // can spawn on any floor tile
    Room,       // can spawn only in a room
}

[System.Serializable]
public class SpawnTable {

    public string id;

    public GameObject prefab;

    [Min(0)]
    public int count = 1;

    [Min(0)]
    public float minDistanceFromPlayer = 0f;

    [Range(0f, 1f)]
    public float spawnChance = 1f;

    public bool guaranteeSpawn = true;

    public GameObject spawnAlso; // spawn another object if this object spawns

    public bool uniqueTile = true;

    public SpawnType spawn;

    // https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@10.5/manual/Operator-InlineAnimationCurve.html
    // increase/decrease spawn chance
    public AnimationCurve floorMultiplier = AnimationCurve.Linear(1, 1, 10, 1);

    public float GetSpawnChance(int floor)
    {
        float multiplier = floorMultiplier.Evaluate(floor);
        return Mathf.Clamp01(spawnChance * multiplier);
    }
}