using UnityEngine;

public abstract class GenericEnemy<TEnamy> : MonoBehaviour  where TEnamy : MonoBehaviour
{
    protected Transform _target; 

    protected abstract void Attack();

    protected abstract void StartBehavior(Transform target);

    public void Initialise(Transform target)
    {
        _target = target; 
    }
}
