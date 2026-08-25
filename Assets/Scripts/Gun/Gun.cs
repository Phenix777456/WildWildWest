using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GunPool))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _hendPosition;
    private GunPool _gunPool;

    private void Awake()
    {
        _gunPool = gameObject.GetComponent<GunPool>();

        gameObject.transform.position = _hendPosition.position;
        gameObject.transform.SetParent(_hendPosition);

    }

    private void Update()
    {
        gameObject.transform.LookAt(_target);
    }
}

