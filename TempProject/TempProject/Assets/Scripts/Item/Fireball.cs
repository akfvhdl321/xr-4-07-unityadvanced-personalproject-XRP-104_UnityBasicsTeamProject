using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("투사체 이동 속도")]
    [SerializeField] private float _speed = 8f;

    [Header("생존 시간")]
    [Tooltip("자동 반환 시간")]
    [SerializeField] private float _lifeTime = 3f;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _timer;

    private FireballPool _pool;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _lifeTime)
        {
            ReturnToPool();
        }
    }

    public void Init(Vector2 dir)
    {
        _direction = dir.normalized;
        _rb.linearVelocity = _direction * _speed;
    }

    public void SetPool(FireballPool pool)
    {
        _pool = pool;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamagable>(out IDamagable target))
        {
            target.TakeDamage(1);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        _rb.linearVelocity = Vector2.zero;

        if (_pool != null)
        {
            _pool.ReturnFireball(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
