using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("이동 속도")]
    [SerializeField] private float _moveSpeed = 2f;

    [Tooltip("이동 거리")]
    [SerializeField] private float _moveDistance = 3f;

    private Vector3 _startPosition;
    private bool _movingRight = true;

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
        if (_movingRight)
        {
            transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, _startPosition) >= _moveDistance)
        {
            _movingRight = !_movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        PlayerHealth health =
        collision.gameObject.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(1);
        }
    }
}