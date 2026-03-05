using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveAxis
    {
        X,
        Y
    }

    [Header("이동 방향")]
    [SerializeField] private MoveAxis _axis = MoveAxis.X;

    [Header("이동 거리")]
    [SerializeField] private float _distance = 3f;

    [Header("이동 속도")]
    [SerializeField] private float _speed = 2f;

    private Vector3 _startPos;
    private Vector3 _targetPos;

    private bool _goingForward = true;

    private void Start()
    {
        _startPos = transform.position;

        if (_axis == MoveAxis.X)
            _targetPos = _startPos + Vector3.right * _distance;
        else
            _targetPos = _startPos + Vector3.up * _distance;
    }

    private void FixedUpdate()
    {
        Vector3 target = _goingForward ? _targetPos : _startPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            _speed * Time.fixedDeltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.02f)
        {
            _goingForward = !_goingForward;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}