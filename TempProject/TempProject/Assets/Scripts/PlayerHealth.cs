using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [Header("체력 설정")]
    [SerializeField] private int _maxHP = 3;

    [Header("피격 후 무적시간")]
    [SerializeField] private float _invincibleTime = 1f;

    private int _currentHP;
    private bool _isInvincible; // 무적 변수
    private float _invincibleTimer;// 무적시간

    public event Action OnDeath;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    private void Update()
    {
        HandleInvincible();
    }

    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;

        _currentHP -= damage;

        Debug.Log("현재 HP : " + _currentHP);

        if (_currentHP <= 0)
        {
            Die();
            return;
        }
        StartInvincible();
    }

    private void StartInvincible()
    {
        _isInvincible = true;
        _invincibleTimer = _invincibleTime;
    }

    private void HandleInvincible()
    {
        if (!_isInvincible) return;

        _invincibleTimer -= Time.deltaTime;

        if (_invincibleTimer <= 0)
        {
            _isInvincible = false;
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        OnDeath?.Invoke();
    }

    public void ResetHealth()
    {
        _currentHP = _maxHP;
        _isInvincible = false;
    }

    
}
