using UnityEngine;

public abstract class EnemyBase: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float agroRadius;

    protected bool isAlive = true;
    protected bool hasBeenDestroyed = false;
    protected float health;
    protected Animator animator;
    [HideInInspector] public EnemyMovement movement;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;
    protected EnemyCombatUI enemyCombatUI;
    protected EnemyLootDropper enemyLootDropper;
    protected PlayerRelics playerRelics;

    protected virtual void Awake()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        enemyCombatUI = GetComponentInChildren<EnemyCombatUI>();
        enemyLootDropper = GetComponent<EnemyLootDropper>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRelics = player.GetComponent<PlayerRelics>();
        }
    }

    public virtual bool TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(health);
        playerRelics?.TriggerLifeSteal(amount);

        animator.SetTrigger("Hurt");
        enemyCombatUI.UpdateHealthBarOnTakeDamage(amount);
        enemyCombatUI.ShowDamagePopUp(amount);

        if (health <= 0f)
        {
            isAlive = false;
            DeathAnimation();
            return false;
        }

        return true;
    }

    protected virtual void DeathAnimation()
    {
        movement.SetCanMove(false);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        bc.enabled = false;
        animator.SetTrigger("Dying");
    }

    // Called using animation event in DeathAnimation
    public virtual void Despawn()
    {
        if (hasBeenDestroyed) return;
        hasBeenDestroyed = true;

        enemyLootDropper.DropLoot();
        Destroy(gameObject);
    }

    // Called using animation event in HurtAnimation
    public virtual void OnHurtFinished()
    {
        movement.SetCanMove(true);
    }

    public float MaxHealth => maxHealth;
    public float Damage => damage;
    public float MoveSpeed => moveSpeed;
    public float AgroRadius => agroRadius;
    public bool IsAlive => isAlive;
}
