using UnityEngine;
using UnityEngine.Pool;

public class SwordPool : GenericPool<Sword>
{
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private ReturnSwordChannel _returnSwordChannel;

    private void OnEnable()
    {
        _returnSwordChannel.ObjectReleased += Release;
    }

    private void OnDisable()
    {
        _returnSwordChannel.ObjectReleased -= Release;
    }

    public Sword Spawn(Vector3 position, Quaternion rotation, Transform parent, Transform target)
    {
        Sword sword = _basePool.Get();
        sword.transform.SetParent(parent);
        sword.transform.SetPositionAndRotation(position, rotation);
        sword.Initialize(target);
        return sword;
    }

    public void Release(Sword sword)
    {
        _basePool.Release(sword);
    }

    protected override void ActionOnGet(Sword sword) => sword.gameObject.SetActive(true);

    protected override void ActionOnRelese(Sword sword) => sword.gameObject.SetActive(false);

    protected override void ActionOnDestroy(Sword sword) => Destroy(sword.gameObject);
}