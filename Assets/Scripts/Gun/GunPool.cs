using UnityEngine;
using UnityEngine.Pool;

public class GunPool : GenericPool<Bullet>
{
    [SerializeField] ReturnBulletChannel _returnBulletChannel;

    private void OnEnable()
    {
        _returnBulletChannel.ObjectReleased += OnReleaseBullet;
    }

    private void OnDisable()
    {
        _returnBulletChannel.ObjectReleased -= OnReleaseBullet;
    }

    protected override void ActionOnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    protected override void ActionOnRelese(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    protected override void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public Bullet SpawnBullet()
    {
        return _basePool.Get();
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        if (bullet.gameObject.activeSelf)
            _basePool.Release(bullet);
    }
}
