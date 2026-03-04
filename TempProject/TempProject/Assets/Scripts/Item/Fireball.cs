using UnityEngine;

/// <summary>
/// Fireball 투사체
/// Enemy 또는 Ground에만 반응
/// </summary>
public class Fireball : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float _speed = 8f;

    [Header("생존 시간")]
    [SerializeField] private float _lifeTime = 3f;

    [SerializeField] private Transform _visual;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _timer;

    private FireballPool _pool;
    private TeamType _ownerTeam;

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

    public void Init(Vector2 dir, TeamType team)
    {
        _direction = dir.normalized;
        _ownerTeam = team;

        _rb.linearVelocity = _direction * _speed;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        _visual.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    public void SetPool(FireballPool pool)
    {
        _pool = pool;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        // Enemy 레이어만 데미지 처리
        if (layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.TryGetComponent<IDamagable>(out IDamagable target))
            {
                if (target.Team != _ownerTeam)
                {
                    target.TakeDamage(1, transform.position);
                }
            }

            ReturnToPool();
        }
        // Ground 레이어 맞으면 제거
        else if (layer == LayerMask.NameToLayer("Ground"))
        {
            ReturnToPool();
        }

        // 나머지는 무시 (Checkpoint, Item 등)
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