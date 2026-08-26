using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericPool<Poleable> : MonoBehaviour where Poleable : MonoBehaviour 
{
    [SerializeField] private Poleable _pooleablePrefab;
    [SerializeField] private int _maxSize;

    protected ObjectPool<Poleable> _basePool;

    private void Start()
    {
        _basePool = new ObjectPool<Poleable>(
            createFunc: () => Instantiate(_pooleablePrefab),
            actionOnGet: (bullet) => ActionOnGet(bullet),
            actionOnRelease: (bullet) => ActionOnRelese(bullet),
            actionOnDestroy: (bullet) => ActionOnDestroy(bullet),
            maxSize: _maxSize
            );
    }

    protected abstract void ActionOnGet(Poleable poleable);

    protected abstract void ActionOnRelese(Poleable poleable);

    protected abstract void ActionOnDestroy(Poleable poleable);
}
