using NUnit.Framework;
using UnityEngine;

public class CombatTests
{
    // asserts weapon data correctly applies to melee weapon
    [Test]
    public void SetWeaponData_UpdatesWeaponIdAndCooldown()
    {
        var weapon = CreateWeapon(id: 2, cooldown: 0.75f);

        Assert.AreEqual(2, weapon.WeaponId);
        Assert.AreEqual(0.75f, weapon.AttackCooldown);
    }

    // asserts weapon is initially ready to attack
    [Test]
    public void CanAttack_ReturnsTrueInitially()
    {
        var weapon = CreateWeapon(id: 1, cooldown: 0.5f);

        Assert.IsTrue(weapon.CanAttack());
    }

    // asserts cooldown prevents repeated attacks immediately
    [Test]
    public void CannotAttackTwiceImmediately()
    {
        var weapon = CreateWeapon(id: 1, cooldown: 1f);

        weapon.Attack(Vector2.right, Vector2.zero, 1f);

        Assert.IsFalse(weapon.CanAttack());
    }

    // asserts ApplyAttackHit does not crash if weapon missing
    [Test]
    public void ApplyAttackHit_WithNoWeapon_DoesNotThrow()
    {
        var combat = CreatePlayerCombat();

        Assert.DoesNotThrow(() =>
        {
            combat.ApplyAttackHit();
        });
    }

    // asserts damage multiplier setter works safely
    [Test]
    public void SetDamageMultiplier_DoesNotThrow()
    {
        var combat = CreatePlayerCombat();

        Assert.DoesNotThrow(() =>
        {
            combat.SetDamageMultiplier(2f);
        });
    }

    // asserts ApplyAttackHit works correctly when weapon assigned
    [Test]
    public void ApplyAttackHit_WithWeaponAssigned_DoesNotThrow()
    {
        var combat = CreatePlayerCombat();
        var weapon = CreateWeapon(id: 1, cooldown: 0.5f);

        combat.SetWeaponForTesting(weapon, 0.5f);

        Assert.DoesNotThrow(() =>
        {
            combat.ApplyAttackHit();
        });
    }

    // asserts enemy health decreases when hit by weapon attack
    [Test]
    public void Enemy_TakesDamage_FromWeaponAttack()
    {
        var enemy = CreateEnemy(startingHealth: 100f, position: Vector2.right);
        var weapon = CreateWeapon(damage: 25f, range: 1f, hitBoxSize: new Vector2(2f, 2f));

        weapon.Attack(Vector2.right, Vector2.zero, 1f);

        Assert.Less(enemy.CurrentHealth, 100f);
    }

    // asserts damage multiplier scales attack damage correctly
    [Test]
    public void Enemy_TakesDamage_WithDamageMultiplier()
    {
        var enemy = CreateEnemy(startingHealth: 100f, position: Vector2.right);
        var weapon = CreateWeapon(damage: 10f, range: 1f, hitBoxSize: new Vector2(2f, 2f));

        weapon.Attack(Vector2.right, Vector2.zero, 2f);

        Assert.AreEqual(80f, enemy.CurrentHealth);
    }

    // asserts player loses health when attacked
    [Test]
    public void Player_TakesDamage_ReducesHealth()
    {
        var player = CreatePlayer(startingHealth: 100f);

        player.TakeDamage(25f);

        Assert.AreEqual(75f, player.GetHealth);
    }

    // asserts player loses health affected by damage taken multiplier when attacked
    [Test]
    public void Player_TakesDamage_UsesDamageTakenMultiplier()
    {
        var player = CreatePlayer(startingHealth: 100f);

        player.SetDamageTakenMultiplier(0.5f);
        player.TakeDamage(20f);

        Assert.AreEqual(90f, player.GetHealth);
    }

    // helper functions
    private static PlayerBase CreatePlayer(float startingHealth)
    {
        var playerObj = new GameObject("Player");

        playerObj.AddComponent<Rigidbody2D>();
        playerObj.AddComponent<Animator>();
        playerObj.AddComponent<SpriteRenderer>();

        var stats = playerObj.AddComponent<PlayerStats>();
        var movement = playerObj.AddComponent<PlayerMovement>();
        var combat = playerObj.AddComponent<PlayerCombat>();

        var weaponObj = new GameObject("Weapon");
        weaponObj.transform.parent = playerObj.transform;
        weaponObj.AddComponent<MeleeWeapon>();

        var playerBase = playerObj.AddComponent<PlayerBase>();

        playerBase.SetHealth(startingHealth);
        playerBase.SetMaxHealth(startingHealth);

        return playerBase;
    }

    private static MeleeWeapon CreateWeapon(
        int id = 1,
        float cooldown = 0.5f,
        float damage = 15f,
        float range = 1f,
        Vector2? hitBoxSize = null)
    {
        var weaponObj = new GameObject("Weapon");
        var weapon = weaponObj.AddComponent<MeleeWeapon>();

        var data = ScriptableObject.CreateInstance<WeaponData>();
        data.id = id;
        data.cooldown = cooldown;
        data.damage = damage;
        data.range = range;
        data.hitBoxSize = hitBoxSize ?? new Vector2(1f, 0.6f);

        weapon.SetWeaponData(data);

        typeof(MeleeWeapon)
            .GetField("enemyLayer",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
            .SetValue(weapon, (LayerMask)LayerMask.GetMask("Enemy"));

        return weapon;
    }


    private static PlayerCombat CreatePlayerCombat()
    {
        var obj = new GameObject("PlayerCombat");
        return obj.AddComponent<PlayerCombat>();
    }


    private static TestEnemy CreateEnemy(float startingHealth, Vector2 position)
    {
        var enemyObj = new GameObject("Enemy");

        enemyObj.AddComponent<Rigidbody2D>();
        enemyObj.AddComponent<BoxCollider2D>();
        enemyObj.AddComponent<Animator>();

        enemyObj.transform.position = position;

        enemyObj.layer = LayerMask.NameToLayer("Enemy"); // set to same layer as weapon

        var enemy = enemyObj.AddComponent<TestEnemy>();
        enemy.ForceInit(startingHealth);

        return enemy;
    }


    // test enemy that bypasses animation/UI dependencies
    private class TestEnemy : EnemyBase
    {
        public void ForceInit(float startingHealth)
        {
            health = startingHealth;
            isAlive = true;
        }

        public override bool TakeDamage(float amount)
        {
            health -= amount;

            if (health <= 0f)
            {
                isAlive = false;
                return false;
            }

            // return false so MeleeWeapon does not try to apply knockback
            return false;
        }

        public float CurrentHealth => health;
    }
}