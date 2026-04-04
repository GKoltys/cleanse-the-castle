using UnityEngine;

public class DarkYellowKnightBossEnemy : EnemyBase, IMapGenInit
{
    private MapGenerator mapGenerator;

    public void Init(MapGenerator generator)
    {
        mapGenerator = generator;
    }

    public override void Despawn()
    {
        // trigger ending cutscene?
        base.Despawn();
    }
}