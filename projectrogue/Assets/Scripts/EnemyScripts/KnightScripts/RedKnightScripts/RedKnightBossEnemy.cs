using UnityEngine;

public class RedKnightBossEnemy : EnemyBase, IMapGenInit
{
    private MapGenerator mapGenerator;

    public void Init(MapGenerator generator)
    {
        mapGenerator = generator;
    }

    public override void Despawn()
    {
        mapGenerator?.OnBossDied();
        base.Despawn();
    }
}