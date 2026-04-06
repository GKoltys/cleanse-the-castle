using UnityEngine;

public class SpikeController : MonoBehaviour, IMapGenInit
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float spikeTiming = 5f;
    [SerializeField] private float spikeDamage = 15f;
    private float nextActivationTime;
    private bool spikesActive = false;

    private PlayerBase player;
    private Animator animator;
    private Collider2D col;
    private MapGenerator dungeon;

    public void Init(MapGenerator controller)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBase>();
        dungeon = controller;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        nextActivationTime = Time.time + spikeTiming;
    }

    public void Update()
    {
        if (Time.time > nextActivationTime)
        {
            animator.SetTrigger("SpikeTrigger");
            nextActivationTime = Time.time + spikeTiming;
        }

        if (spikesActive)
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 0.8f), 0f, playerLayer);

            if (hit != null)
            {
                player.TakeDamage(spikeDamage);
            }
        }
    }

    // All called through animation events
    public void SpikesHurtOn()
    {
        spikesActive = true;
    }

    public void SpikesHurtOff()
    {
        spikesActive = false;
    }

    public void SetSpikesDown()
    {
        animator.SetTrigger("SpikeLeave");
    }

    public void SpikesIdle()
    {
        animator.SetTrigger("SpikeIdle");
    }
}
