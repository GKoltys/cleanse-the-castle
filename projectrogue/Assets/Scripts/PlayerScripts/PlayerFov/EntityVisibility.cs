using UnityEngine;

public class EntityVisibility : MonoBehaviour
{
    private PlayerVisibilityMesh visibility;
    private SpriteRenderer[] rendererArray;

    private void Start()
    {
        visibility = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerVisibilityMesh>();
        rendererArray = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (visibility == null) return;
        bool visible = visibility.IsPointVisible(transform.position);
        foreach (SpriteRenderer renderer in rendererArray)
        {
            renderer.enabled = visible;
        }
    }
}
