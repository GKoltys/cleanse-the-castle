using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private LayerMask enemyLayer;

    private float nextAttackTime;

    public bool CanAttack()
    {
        return Time.time >= nextAttackTime;
    }

    public void Attack(Vector2 dir, Vector2 origin, float damageMultiplier)
    {
        if (!CanAttack()) return;
        nextAttackTime = Time.time + weaponData.cooldown;

        Vector2 center = origin + dir * weaponData.range;

        var hits = Physics2D.OverlapBoxAll(center, weaponData.hitBoxSize, 0f, enemyLayer);
        foreach (var h in hits)
        {
            bool isAlive = h.GetComponentInParent<EnemyBase>().TakeDamage(weaponData.damage * damageMultiplier);

            if (isAlive)
            {
                Vector2 kbDir = ((Vector2)h.transform.position - origin).normalized;
                h.GetComponentInParent<EnemyKnockback>().ApplyKnockback(kbDir, weaponData.knockbackForce);
            }
        }
    }

    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
    }

    public float AttackCooldown => weaponData.cooldown;
    public int WeaponId => weaponData.id;
}
