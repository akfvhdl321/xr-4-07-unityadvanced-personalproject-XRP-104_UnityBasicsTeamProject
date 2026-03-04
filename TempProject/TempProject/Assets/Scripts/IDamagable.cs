using UnityEngine;

// 데미지를 받을 수 있는 대상 인터페이스
public interface IDamagable
{
    TeamType Team { get; }     // 소속 팀
    void TakeDamage(int damage, Vector2 attackerPosition);
}