using UnityEngine;

public class BlueKnightBossEnemy : EnemyBase, IMapGenInit
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