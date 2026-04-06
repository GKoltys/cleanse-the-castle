using UnityEngine;
using UnityEngine.SceneManagement;

public class DarkYellowKnightBossEnemy : EnemyBase, IMapGenInit
{
    private MapGenerator mapGenerator;
    private bool despawn = true;

    public void Init(MapGenerator generator)
    {
        mapGenerator = generator;
    }

    public override void Despawn()
    {
        if (despawn)
        {
            despawn = false;
            SaveController.Instance.RequestLoad();
            SceneManager.LoadSceneAsync("EndingScene");
            base.Despawn();
        }
    }
}