using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpawner : MonoBehaviour
{
    [SerializeField] private SwordPool _swordPool;
    [SerializeField] private float _forwardOffset; 
    [SerializeField] private Transform _target;
    [SerializeField] private int _swordsPerWave;
    [SerializeField] private int _waveCount;
    [SerializeField] private float _delayBetweenWaves;
    [SerializeField] private float _spawnRadius;

    private Coroutine _spawnRoutine;

    private List<Sword> _swords;

    public event Action SwordsSpawned;

    private void Awake()
    {
        _swords = new List<Sword>();
    }

    public void StartSpawning()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);

        _spawnRoutine = StartCoroutine(SpawnWavesRoutine());
    }

    private IEnumerator SpawnWavesRoutine()
    {
        for (int wave = 0; wave < _waveCount; wave++)
        {
            SpawnWave(wave);
            yield return new WaitForSeconds(_delayBetweenWaves);
        }

        SwordsSpawned?.Invoke();
    }

    private void SpawnWave(int waveIndex)
    {
        float angleStep = 360f / _swordsPerWave;
        float currentRadius = _spawnRadius + waveIndex;
        float angleOffset = waveIndex * (angleStep / 2f);

        Vector3 centerPosition = _target.position + _target.forward * _forwardOffset;
        Quaternion spawnRotation = _target.rotation * Quaternion.Euler(new Vector3(0, 90, 0));

        for (int i = 0; i < _swordsPerWave; i++)
        {
            float angleRad = (angleStep * i + angleOffset) * Mathf.Deg2Rad;

            Vector3 circleOffset = _target.right * Mathf.Cos(angleRad) * currentRadius
                                  + _target.up * Mathf.Sin(angleRad) * currentRadius;

            Vector3 spawnPosition = centerPosition + circleOffset;

            _swords.Add(_swordPool.Spawn(spawnPosition, spawnRotation, transform, _target));
        }
    }

    public List<Sword> GetSwordsList()
    {
        if (_swords.Count > 0) 
            return _swords;

        return null;
    }

    private void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }
}