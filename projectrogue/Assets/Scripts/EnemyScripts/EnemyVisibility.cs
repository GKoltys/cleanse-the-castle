using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
    private PlayerVisibilityMesh visibility;
    private SpriteRenderer[] rendererArray;
    private Canvas[] canvasArray;
    private EnemyBase enemyBase;

    private void Start()
    {
        visibility = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerVisibilityMesh>();
        rendererArray = GetComponentsInChildren<SpriteRenderer>();
        canvasArray = GetComponentsInChildren<Canvas>();
        enemyBase = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        if (visibility == null || !enemyBase.IsAlive) return;
        bool visible = visibility.IsPointVisible(transform.position);
        foreach (SpriteRenderer renderer in rendererArray)
        {
            renderer.enabled = visible;
        }
        foreach (Canvas canvas in canvasArray)
        {
            canvas.enabled = visible;
        }
    }
}