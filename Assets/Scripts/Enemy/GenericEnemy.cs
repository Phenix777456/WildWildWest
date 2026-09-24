using UnityEngine;
using UnityEngine.Playables;

public abstract class GenericEnemy<TEnamy> : MonoBehaviour  where TEnamy : MonoBehaviour
{
    [SerializeField] private bool _isPlayable;
    protected Transform _target;

    protected abstract void Attack();

    protected abstract void StartBehavior(Transform target);

    public void Initialise(Transform target)
    {
        _target = target;

        if (_isPlayable)
            StartBehavior(_target);
    }
}
