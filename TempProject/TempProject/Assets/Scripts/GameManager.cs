using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Vector3 _currentCheckpointPosition;

    private PlayerRespawn _playerRespawn;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // 플레이어 시작시 호출
    public void RegisterPlayer(PlayerRespawn respawn, PlayerHealth health)
    {
        _playerRespawn = respawn;
        _playerHealth = health;

        _playerHealth.OnDeath += HandlePlayerDeath;
    }
    // 체크포인트 저장
    public void SetCheckpoint(Vector3 position)
    {
        _currentCheckpointPosition = position;

        Debug.Log("체크포인트 저장 위치: " + position);
    }

    // 플레이어 사망 시 호출
    private void HandlePlayerDeath()
    {
        Debug.Log("GameManager 사망 감지");

        _playerRespawn.Respawn(_currentCheckpointPosition);
        _playerHealth.ResetHealth();
    }

}
