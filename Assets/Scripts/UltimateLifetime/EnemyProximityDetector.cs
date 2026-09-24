using System;
using UnityEngine;

public class EnemyProximityDetector : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 20f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private int _countEnemy = 20;
    public event Action<Collider> EnemyDetected;
    private Collider[] _hitsBuffer;

    private void Awake()
    {
        _hitsBuffer = new Collider[_countEnemy];
    }

    public void DetectEnemies()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position,
            _detectionRadius,
            _hitsBuffer,
            _enemyLayerMask);

        for (int i = 0; i < hitCount; i++)
            EnemyDetected?.Invoke(_hitsBuffer[i]);
    }
}
