using UnityEngine;


public class EnemyNearPool : GenericPool<EnemyNear>
{
    protected override void ActionOnGet(EnemyNear poolable)
    {
        poolable.gameObject.SetActive(true);
    }

    protected override void ActionOnRelease(EnemyNear poolable)
    {
        poolable.gameObject.SetActive(false);
    }

    protected override void ActionOnDestroy(EnemyNear poolable)
    {
        Destroy(poolable.gameObject);
    }

    public EnemyNear Spawn()
    {
        return _basePool.Get();
    }
}
