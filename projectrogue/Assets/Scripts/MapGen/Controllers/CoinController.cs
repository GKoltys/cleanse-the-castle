using UnityEngine;

public class CoinController : MonoBehaviour, IMapGenInit
{
    private Animator animator;
    private Collider2D col;
    private MapGenerator dungeon;
    private bool collected = false;

    private PlayerBase player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
    }

    public void Init(MapGenerator controller)
    {
        dungeon = controller;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dungeon == null) return;

        if (!other.CompareTag("Player")) return;

        collected = true;
        SoundEffectManager.Play(SoundGroupName.COIN);
        if (col) col.enabled = false;

        player.CoinCollected(1);

        if (animator != null && collected)
        {
            // animation transitions - https://docs.unity3d.com/6000.3/Documentation/Manual/class-Transition.html
            animator.SetTrigger("CoinGet");
        }

    }

    public void Despawn()
    {
        // destroy for now, if it affects performance move to object pooling?
        Destroy(gameObject);
    }
}
