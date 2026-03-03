using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("이동 설정")]
    [SerializeField] private float _speed = 8f;

    [Header("생존 시간")]
    [SerializeField] private float _lifeTime = 3f;

    [SerializeField] private Transform _visual;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;   // 스프라이트 렌더러 참조
    private Vector2 _direction;
    private float _timer;

    private FireballPool _pool;
    private TeamType _ownerTeam;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>(); // 추가
    }

    private void OnEnable()
    {
        Debug.Log("Fireball OnEnable");
        _timer = 0f;
    }

    private void Update()
    {
        Debug.Log("Fireball Update 실행");
        _timer += Time.deltaTime;

        if (_timer >= _lifeTime)
        {
            Debug.Log("LifeTime 초과");
            ReturnToPool();
        }
    }

    public void Init(Vector2 dir, TeamType team)
    {
        _direction = dir.normalized;
        _ownerTeam = team;

        _rb.linearVelocity = _direction * _speed;

        // 스프라이트가 위쪽 기준이므로 -90도 보정
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        _visual.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    public void SetPool(FireballPool pool)
    {
        _pool = pool;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamagable>(out IDamagable target))
        {
            if (target.Team == _ownerTeam)
                return;

            target.TakeDamage(1);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        Debug.Log("🔥 ReturnToPool 호출됨");
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