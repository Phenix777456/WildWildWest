using UnityEngine;

public class EnemyNearSpawner : MonoBehaviour
{
    [SerializeField] private EnemyNearPool _enemyNearPool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _target;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        SpawnSides(6,2);
    }

    public void SpawnSides(int count, float distance)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = GetSidePosition(i, distance);
            EnemyNear enemy = _enemyNearPool.Spawn();
            enemy.Initialise(_target);
            enemy.transform.SetPositionAndRotation(position, _spawnPoint.rotation);
        }
    }

    private Vector3 GetSidePosition(int index, float distance)
    {
        int side = index % 2 == 0 ? 1 : -1;
        int step = index / 2 + 1;
        float offset = side * step * distance;

        return _spawnPoint.position + _spawnPoint.right * offset;
    }
}
