using System;
using UnityEngine;

public class EnemyNearSpawner : MonoBehaviour , IEnemySpawner
{
    [SerializeField] private EnemyNearPool _enemyNearPool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _target;

    public event Action EnemyDied;

    public void Spawn(int enemyCount, float enemyDistance)
    {
        SpawnSides(enemyCount, enemyDistance);
    }

    public void SpawnSides(int count, float distance)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = GetSidePosition(i, distance);
            EnemyNear enemy = _enemyNearPool.Spawn();
            enemy.Initialise(_target);
            SetDeadEvent(enemy);
            enemy.transform.SetPositionAndRotation(position, _spawnPoint.rotation);
        }
    }

    private void SetDeadEvent(EnemyNear enemy)
    {
        var damageable = enemy.Damageable;

        if (damageable != null)
        {
            void OnDied()
            {
                damageable.Died -= OnDied;
                OnEnemyDied(enemy);
            }

            damageable.Died += OnDied;
        }
    }

    private void OnEnemyDied(EnemyNear enemy)
    {
        _enemyNearPool.ReleaseEnemy(enemy);
        EnemyDied?.Invoke();
    }

    private Vector3 GetSidePosition(int index, float distance)
    {
        int side = index % 2 == 0 ? 1 : -1;
        int step = index / 2 + 1;
        float offset = side * step * distance;

        return _spawnPoint.position + _spawnPoint.right * offset;
    }
}
