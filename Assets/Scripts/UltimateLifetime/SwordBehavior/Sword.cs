using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Renderer), typeof(SwordTrigger))]
public class Sword : MonoBehaviour
{
    [SerializeField] private bool _isInHend;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _targetAngleOffset = 90;
    [SerializeField] private float _dellay;
    [SerializeField] private ReturnSwordChannel _returnSwordChannel;
    [SerializeField] private ParticleSystem _burstEffect;

    private Renderer _renderer;
    private Rigidbody _rigitbody;
    private MaterialPropertyBlock _propertyBlock;
    private Transform _target;
    private float _timer;
    private SwordTrigger _swordTrigger; 

    public event Action<Sword> AppearFinished;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _swordTrigger = GetComponent<SwordTrigger>();

        _rigitbody = GetComponent<Rigidbody>();

        _propertyBlock = new MaterialPropertyBlock();

        if (_isInHend == false)
        {
            StartCoroutine(DellayDestroy(_dellay));
        }
    }

    private void OnEnable()
    {
        _swordTrigger.GroundTouched += OnGroundTouched;
    }

    private void OnDisable()
    {
        _swordTrigger.GroundTouched -= OnGroundTouched;
    }

    private Quaternion _baseRotation;

    public void Initialize(Transform target)
    {
        _target = target;
        _baseRotation = transform.rotation; 
        _timer = 0f;                        
      
    }

    private void OnGroundTouched()
    {
        ParticleSystem thisParticle  =  Instantiate(_burstEffect);

        thisParticle.transform.position = gameObject.transform.position;

        _returnSwordChannel.Release(this);
    }

    public void SetSwordInHend(Transform target)
    {
        gameObject.transform.SetParent(target.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }

    private void FaceTarget()
    {
        transform.rotation = _baseRotation * Quaternion.Euler(0f, 0f, _targetAngleOffset);
    }

    private void Update()
    {
        if (_isInHend)
            return;

        if (_target == null)
            return;

        if (_rigitbody.isKinematic == true)
            FaceTarget();
    }

    private IEnumerator DellayDestroy(float dellay)
    {
        yield return new WaitForSeconds(dellay);

        _returnSwordChannel.Release(this);
    }
}