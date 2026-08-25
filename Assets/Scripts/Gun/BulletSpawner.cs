using System;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private AimTargetProvider _aimTargetProvider;
    [SerializeField] private GunPool _bulletPool;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _bulletSpeed = 40f;

    public event Action PointsHendled;

    public void Fire()
    {
        Vector3 direction = (_aimTargetProvider.CurrentAimPoint - _muzzle.position).normalized;
        Spawn(direction);
    }

    private void Spawn(Vector3 direction)
    {
        Bullet bullet = _bulletPool.SpawnBullet();

        bullet.transform.position = _muzzle.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.GetComponent<BulletTrigger>().EnemyHited += OnEnemyHited;

        bullet.Launch(direction, _bulletSpeed);
    }

    private void OnEnemyHited(BulletTrigger bulletTrigger)
    {
        PointsHendled?.Invoke();
        
        bulletTrigger.EnemyHited -= OnEnemyHited;
    }
}