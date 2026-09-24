using UnityEngine;

[RequireComponent(typeof(EnemyProximityDetector))]
public class FreezeTrap : MonoBehaviour
{
    [SerializeField] private EnemyProximityDetector _detector;

    private void OnEnable()
    {
        _detector.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _detector.EnemyDetected -= OnEnemyDetected;
    }

    private void OnEnemyDetected(Collider other)
    {
        EnamyMover mover = other.gameObject.GetComponent<EnamyMover>();
        mover.Freeze();
    }
}
