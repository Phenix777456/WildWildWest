using UnityEngine;
using UnityEngine.Pool;

public class SwordPool : MonoBehaviour
{
    [SerializeField] private Sword _swordPrefab;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 50;
    [SerializeField] private ReturnSwordChannel _returnSwordChannel;

    private ObjectPool<Sword> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Sword>(
            createFunc: CreateSword,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroySword,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

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
        Sword sword = _pool.Get();
        sword.transform.SetParent(parent);
        sword.transform.SetPositionAndRotation(position, rotation);
        sword.Initialize(target);
        return sword;
    }

    public void Release(Sword sword)
    {
        _pool.Release(sword);
    }

    private Sword CreateSword() => Instantiate(_swordPrefab);

    private void OnGet(Sword sword) => sword.gameObject.SetActive(true);

    private void OnRelease(Sword sword)
    {
        sword.gameObject.SetActive(false);
    }

    private void OnDestroySword(Sword sword) => Destroy(sword.gameObject);
}