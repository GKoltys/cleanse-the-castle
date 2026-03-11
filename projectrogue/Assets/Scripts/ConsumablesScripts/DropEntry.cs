using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public GameObject itemPrefab;
    [UnityEngine.Range(0f, 100f)] public float weight;
}
