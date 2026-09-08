using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyNear : GenericEnemy<EnemyNear>
{
    [SerializeField] private EnamyMover _mover;

    private void OnEnable()
    {
        _mover.TargetReached += OnTargetReached;
    }

    private void Start()
    {
        StartBehavior(_target);
    }

    private void OnDisable()
    {
        _mover.TargetReached -= OnTargetReached;
    }

    private void OnTargetReached()
    {
        Attack();

        StartCoroutine(AttackDellay(1));
    }


    protected override void Attack()
    {
        Debug.Log("Attack");
    }

    protected override void StartBehavior(Transform target)
    {
        if (_mover.IsMoving == false)
            _mover.MoveTo(target);
    }

    private IEnumerator AttackDellay(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        if (_mover.HasReachedTarget(_target.position))
        {
            Attack();

            StartCoroutine(AttackDellay(1));
        }
        else
        {
            _mover.MoveTo(_target);
        }
    }
}
