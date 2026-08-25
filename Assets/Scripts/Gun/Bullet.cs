using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private ReturnBulletChannel _returnBulletChannel;
    [SerializeField] private float _lifeTime;

    private Rigidbody _rigidbody;
    private Coroutine _returnRoutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _returnRoutine = StartCoroutine(LifeTime(_lifeTime));
    }

    private void OnDisable()
    {
        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
        }
    }

    public void Launch(Vector3 direction, float speed)
    {
        _rigidbody.linearVelocity = direction.normalized * speed;
    }

    private IEnumerator LifeTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        _returnBulletChannel.Release(this);
        Debug.Log("Relesases count");

    }
}
