using UnityEngine;

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
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

        // 플레이어가 아래 방향으로 떨어지고 있을 때만 인정
        if (playerRb.linearVelocity.y <= 0)
        {
            // 적 사망
            _enemyHealth.TakeDamage(1);

            // 플레이어 튕김
            playerRb.linearVelocity = new Vector2(
                playerRb.linearVelocity.x,
                _bounceForce);
        }
    }
}
