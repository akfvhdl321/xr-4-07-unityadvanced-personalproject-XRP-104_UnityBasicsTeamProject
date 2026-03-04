using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("이동 속도")]
    [SerializeField] private float _moveSpeed = 2f;

    [Tooltip("이동 거리")]
    [SerializeField] private float _moveDistance = 3f;

    [Header("낙사 방지 설정")]
    [SerializeField] private float _groundCheckDistance = 0.5f;
    [SerializeField] private LayerMask _groundLayer;

    private BoxCollider2D _col;

    private Vector3 _startPosition;
    private bool _movingRight = true;

    private void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!IsGroundAhead())
        {
            _movingRight = !_movingRight;
        }

        Vector2 dir = _movingRight ? Vector2.right : Vector2.left;

        transform.Translate(dir * _moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamagable>(out IDamagable target))
            return;

        if (target.Team == TeamType.Enemy)
            return;

        // 플레이어 위치가 Enemy 위쪽이면 무시 (스톰프 우선)
        if (other.transform.position.y > transform.position.y + 0.2f)
            return;

        target.TakeDamage(1, transform.position);
    }

    private bool IsGroundAhead()
    {
        Vector2 origin = new Vector2(
            _col.bounds.center.x + (_movingRight ? _col.bounds.extents.x : -_col.bounds.extents.x),
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