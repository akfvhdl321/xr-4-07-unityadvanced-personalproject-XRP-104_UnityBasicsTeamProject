using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyStomp : MonoBehaviour
{
    private EnemyHealth _enemyHealth;

    [Header("플레이어 튕김 힘")]
    [SerializeField] private float _bounceForce = 5f;

    private void Awake()
    {
        _enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerController>(out var player))
            return;

        Rigidbody2D playerRb = player._rb;

        // 아래 방향으로 떨어지고 있을 때만
        if (playerRb.linearVelocity.y < -0.1f)
        {
            _enemyHealth.TakeDamage(1, other.transform.position);

            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                _bounceForce
            );
        }
    }
}
