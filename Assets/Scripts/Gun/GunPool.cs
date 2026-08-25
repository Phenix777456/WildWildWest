using UnityEngine;
using UnityEngine.Pool;

public class GunPool : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] ReturnBulletChannel _returnBulletChannel;

    private ObjectPool<Bullet> _gunPool;

    private void OnEnable()
    {
        _returnBulletChannel.ObjectReleased += OnReleaseBullet;
    }

    void Start()
    {
        _gunPool = new ObjectPool<Bullet>(
            createFunc: () => Instantiate(_bulletPrefab),
            actionOnGet: (bullet) => ActionOnGet(bullet),
            actionOnRelease: (bullet) => ActionOnRelese(bullet),
            actionOnDestroy: (bullet) => ActionOnDestroy(bullet),
            maxSize: 10
            );
    }

    private void OnDisable()
    {
        _returnBulletChannel.ObjectReleased -= OnReleaseBullet;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelese(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public Bullet SpawnBullet()
    {
        return _gunPool.Get();
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        if (bullet.gameObject.activeSelf)
            _gunPool.Release(bullet);
    }
}
