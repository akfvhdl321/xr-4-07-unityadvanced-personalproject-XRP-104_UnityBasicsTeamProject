using UnityEngine;

/// <summary>
/// 기본은 Patrol
/// 플레이어 감지 시 Follow
/// 플랫폼 끝에서 낙사하지 않음
/// 스케일 유지 + 방향 반전 안정화
/// </summary>
public class FollowEnemy : MonoBehaviour
{
    private enum State
    {
        Patrol,
        Follow
    }

    [Header("공통 이동 설정")]
    [Tooltip("기본 이동 속도")]
    [SerializeField] private float _moveSpeed = 2f;

    [Header("감지 설정")]
    [Tooltip("플레이어 감지 거리")]
    [SerializeField] private float _detectRange = 5f;

    [Header("Patrol 설정")]
    [Tooltip("좌우 이동 거리")]
    [SerializeField] private float _patrolDistance = 3f;

    [Header("낙사 방지 설정")]
    [Tooltip("바닥 감지 거리")]
    [SerializeField] private float _groundCheckDistance = 0.5f;

    [Tooltip("바닥 레이어")]
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody2D _rb;
    private Transform _player;
    private BoxCollider2D _col;

    private State _currentState;
    private Vector2 _startPosition;
    private int _direction = 1; // 1 = 오른쪽, -1 = 왼쪽
    private Vector3 _originalScale;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();

        _originalScale = transform.localScale;

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
            _player = player.transform;

        _startPosition = transform.position;
        _currentState = State.Patrol;
    }

    private void FixedUpdate()
    {
        // 상태 결정
        if (_player != null)
        {
            float distance = Vector2.Distance(transform.position, _player.position);

            if (distance <= _detectRange)
                _currentState = State.Follow;
            else
                _currentState = State.Patrol;
        }
        else
        {
            _currentState = State.Patrol;
        }

        // 상태 실행
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
    // Patrol 동작
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
    // Follow 동작
    // ===============================
    private void Follow()
    {
        float delta = _player.position.x - transform.position.x;

        // 너무 가까울 때 방향이 0 되는 현상 방지
        if (Mathf.Abs(delta) > 0.1f)
        {
            _direction = delta > 0 ? 1 : -1;
        }

        if (!IsGroundAhead())
        {
            _direction *= -1;
        }

        ApplyMovement();
    }

    // ===============================
    // 실제 이동 적용 (공통 처리)
    // ===============================
    private void ApplyMovement()
    {
        _rb.linearVelocity = new Vector2(_direction * _moveSpeed, _rb.linearVelocity.y);

        transform.localScale = new Vector3(
            _originalScale.x * _direction,
            _originalScale.y,
            _originalScale.z
        );
    }

    // ===============================
    // 바닥 감지 (Collider 기준 안정 방식)
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
}