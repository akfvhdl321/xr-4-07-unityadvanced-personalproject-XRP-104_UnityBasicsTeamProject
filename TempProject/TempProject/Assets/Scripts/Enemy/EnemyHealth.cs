using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [Header("적 체력 설정")]
    [SerializeField] private int _maxHP = 1;

    private int _currentHP;

    // 적 팀 반환
    public TeamType Team => TeamType.Enemy;

    void Awake()
    {
        _currentHP = _maxHP;
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        _currentHP -= damage;

        if (_currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("적 사망");
        Destroy(gameObject);
    }
}