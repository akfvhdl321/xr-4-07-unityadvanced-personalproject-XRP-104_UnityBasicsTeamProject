using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Follow
    }

    [Header("이동 속도")]
    [SerializeField] private float _moveSpeed = 2f;

    [Header("플레이어 감지 거리")]
    [SerializeField] private float _detectRange = 5f;

    [Header("플레이어 높이 감지 범위")]
    [SerializeField] private float _heightTolerance = 1.5f;

    [Header("Patrol 이동 거리")]
    [SerializeField] private float _patrolDistance = 3f;

    [Header("낙사 방지")]
    [SerializeField] private float _groundCheckDistance = 0.7f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("데미지 쿨타임")]
    [SerializeField] private float _damageCooldown = 0.2f;

    private Rigidbody2D _rb;
    private Transform _player;
    private BoxCollider2D _col;

    private State _currentState;

    private Vector2 _startPosition;
    private int _direction = 1;

    private Vector3 _originalScale;

    private float _lastDamageTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();

        _originalScale = transform.localScale;

        _startPosition = transform.position;

        _currentState = State.Patrol;
    }

    // GameManager에서 Player 전달
    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    private void FixedUpdate()
    {
        if (_player == null)
            return;

        UpdateState();

        switch (_currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Follow:
                Follow();
                break;
        }
    }

    // ===============================
    // 상태 업데이트
    // ===============================

    private void UpdateState()
    {
        float xDistance = Mathf.Abs(
            transform.position.x - _player.position.x
        );

        float yDistance = Mathf.Abs(
            transform.position.y - _player.position.y
        );

        if (xDistance <= _detectRange && yDistance <= _heightTolerance)
        {
            _currentState = State.Follow;
        }
        else
        {
            if (_currentState == State.Follow)
            {
                _startPosition = transform.position;
            }

            _currentState = State.Patrol;
        }
    }

    // ===============================
    // Patrol
    // ===============================

    private void Patrol()
    {
        float movedDistance = transform.position.x - _startPosition.x;

        if (Mathf.Abs(movedDistance) >= _patrolDistance || !IsGroundAhead())
        {
            _direction *= -1;
        }

        ApplyMovement();
    }

    // ===============================
    // Follow
    // ===============================

    private void Follow()
    {
        float delta = _player.position.x - transform.position.x;

        int targetDir = delta > 0 ? 1 : -1;

        _direction = targetDir;

        // 낭떠러지면 멈춤
        if (!IsGroundAhead())
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            return;
        }

        ApplyMovement();
    }

    // ===============================
    // 이동 처리
    // ===============================

    private void ApplyMovement()
    {
        _rb.linearVelocity = new Vector2(
            _direction * _moveSpeed,
            _rb.linearVelocity.y
        );

        transform.localScale = new Vector3(
            _originalScale.x * _direction,
            _originalScale.y,
            _originalScale.z
        );
    }

    // ===============================
    // 바닥 체크
    // ===============================

    private bool IsGroundAhead()
    {
        Vector2 origin = new Vector2(
            _col.bounds.center.x + (_direction * _col.bounds.extents.x),
            _col.bounds.min.y - 0.05f
        );

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            _groundCheckDistance,
            _groundLayer
        );

        Debug.DrawRay(origin, Vector2.down * _groundCheckDistance, Color.red);

        return hit.collider != null;
    }

    // ===============================
    // 플레이어 충돌
    // ===============================

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
            return;
        // 플레이어 발 위치
        float playerBottom = other.bounds.min.y;
        // 적 머리 위치
        float enemyTop = _col.bounds.max.y;
        // 플레이어가 적 머리 위에 있으면 밟기 상황
        if (playerBottom > enemyTop - 0.05f)
            return;

        if (Time.time - _lastDamageTime < _damageCooldown)
            return;

        _lastDamageTime = Time.time;

        player.GetComponent<IDamagable>()?.TakeDamage(1, transform.position);
    }
}