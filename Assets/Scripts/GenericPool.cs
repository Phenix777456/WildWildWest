using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericPool<Poleable> : MonoBehaviour where Poleable : MonoBehaviour 
{
    [SerializeField] private Poleable _pooleablePrefab;
    [SerializeField] private int _maxSize;

    protected ObjectPool<Poleable> _basePool;

    private void Awake()
    {
        _basePool = new ObjectPool<Poleable>(
            createFunc: () => Instantiate(_pooleablePrefab),
            actionOnGet: (poolable) => ActionOnGet(poolable),
            actionOnRelease: (poolable) => ActionOnRelease(poolable),
            actionOnDestroy: (poolable) => ActionOnDestroy(poolable),
            maxSize: _maxSize
            );
    }

    protected abstract void ActionOnGet(Poleable poleable);

    protected abstract void ActionOnRelease(Poleable poleable);

    protected abstract void ActionOnDestroy(Poleable poleable);
}
