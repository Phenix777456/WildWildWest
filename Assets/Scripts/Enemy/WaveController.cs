using System;
using UnityEngine;


public interface IEnemySpawner
{
    event Action EnemyDied;

    void Spawn(int enemyCount, float spacing);
}

public class WaveController : MonoBehaviour
{
    [SerializeField] private WaveConfig[] _waves;
    [SerializeField] private EnemyNearSpawner _spawner;

    public event Action<int> WaveStarted;
    public event Action AllWavesCompleted;

    private int _currentWaveIndex = -1;
    private int _aliveEnemiesInWave;
    private bool _isRunning;

    private void OnEnable()
    {
        _spawner.EnemyDied += OnEnemyDied;
    }

    private void OnDisable()
    {
        _spawner.EnemyDied -= OnEnemyDied;
    }

    private void Start()
    {
        StartNextWave();
    }

    private void OnEnemyDied()
    {
        if (_isRunning == false)
            return;

        _aliveEnemiesInWave--;

        if (_aliveEnemiesInWave <= 0)
            StartNextWave();
    }

    private void StartNextWave()
    {
        _currentWaveIndex++;

        if (_currentWaveIndex >= _waves.Length)
        {
            _isRunning = false;
            AllWavesCompleted?.Invoke();
            return;
        }

        WaveConfig wave = _waves[_currentWaveIndex];
        _aliveEnemiesInWave = wave.EnemyCount;
        _isRunning = true;

        WaveStarted?.Invoke(_currentWaveIndex);
        _spawner.Spawn(wave.EnemyCount, wave.Spacing);
    }
}
