using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Waves/Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] private int _enemyCount;
    [SerializeField] private float _spacing;

    public int EnemyCount => _enemyCount;
    public float Spacing => _spacing;
}
