using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyNear : GenericEnemy<EnemyNear>
{
    [SerializeField] private EnamyMover _mover;
    [SerializeField] private HealthController _health;
    [SerializeField] private AnimatorController _animator;

    public bool IsAttacking { get; private set; } 

    public IDamageable Damageable => _health;

    private void OnEnable()
    {
        _mover.TargetReached += OnTargetReached;
    }

    private void OnDisable()
    {
        _mover.TargetReached -= OnTargetReached;
    }

    private void OnTargetReached()
    {
        IsAttacking = true;

        Attack();

        StartCoroutine(AttackDellay(1));
    }


    protected override void Attack()
    {
        _mover.SincRotation(_target);
        _animator.SetSwordPoseTrigger();
    }

    protected override void StartBehavior(Transform target)
    {
        if (_mover.IsMoving == false)
            _mover.MoveTo(target);
    }

    private IEnumerator AttackDellay(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        IsAttacking = false;

        if (_mover.HasReachedTarget(_target.position))
        {
            IsAttacking = true;

            Attack();

            StartCoroutine(AttackDellay(1));
        }
        else
        {
            _mover.MoveTo(_target);
        }
    }
}
