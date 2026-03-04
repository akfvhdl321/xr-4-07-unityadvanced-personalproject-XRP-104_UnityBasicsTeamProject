using System;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [Header("체력 설정")]
    [SerializeField] private int _maxHP = 3;

    [Header("피격 후 무적시간")]
    [SerializeField] private float _invincibleTime = 1f;

    [SerializeField] private SpriteRenderer _sprite;

    private int _currentHP;
    private bool _isInvincible;

    private bool _isDead;

    public event Action OnDeath;

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;

    // 플레이어 팀 반환
    public TeamType Team => TeamType.Player;

    private void Awake()
    {
        _currentHP = _maxHP;
        _sprite = GetComponent<SpriteRenderer>();
    }


    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (_isInvincible) return;
        if (_isDead) return;

        _currentHP -= damage;

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
            controller.ApplyKnockback(attackerPosition);

        if (_currentHP <= 0)
        {
            Die();
            return;
        }

        StartInvincible(_invincibleTime);
    }

    private void StartInvincible(float duration)
    {
        StartCoroutine(InvincibleRoutine(duration));
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        _isInvincible = true;

        float timer = 0f;

        while (timer < duration)
        {
            _sprite.enabled = !_sprite.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        _sprite.enabled = true;
        _isInvincible = false;
    }


    private void Die()
    {
        if (_isDead) return;   // 안전장치

        _isDead = true;

        Debug.Log("플레이어 사망");
        OnDeath?.Invoke();
        GetComponent<PlayerController>().DisableFireMode();
    }

    public void ResetHealth()
    {
        _currentHP = _maxHP;
        _isInvincible = false;
        _isDead = false;
    }

    public IEnumerator RespawnInvincibleRoutine(float duration)
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true);

        _isInvincible = true;

        float timer = 0f;

        while (timer < duration)
        {
            _sprite.enabled = !_sprite.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        _sprite.enabled = true;
        _isInvincible = false;

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            false);
    }
}