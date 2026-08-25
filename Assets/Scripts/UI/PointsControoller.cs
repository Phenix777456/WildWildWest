using System;
using UnityEngine;

public class PointsControoller : MonoBehaviour
{
    [SerializeField] private BulletSpawner _bulletSpawner2;
    [SerializeField] private BulletSpawner _bulletSpawner1;
    [SerializeField] private UIUltBarController _barController;

    private int _baseCount = 10;

    [Header("Skils Costs:")]
    [SerializeField] private float _ultimateCost = 1;

    public event Action SpBarFilled;

    private void OnEnable()
    {
        _bulletSpawner1.PointsHendled += OnPointsHendled;
        _bulletSpawner2.PointsHendled += OnPointsHendled;
    }

    private void OnDisable()
    {
        _bulletSpawner1.PointsHendled -= OnPointsHendled;
        _bulletSpawner2.PointsHendled -= OnPointsHendled;
    }

    private void OnPointsHendled()
    {
        _barController.ChangeHpBar(_baseCount);

        if( _barController.CheckHPFill() >= _ultimateCost)
        {
            SpBarFilled.Invoke();
        }
    }
}
