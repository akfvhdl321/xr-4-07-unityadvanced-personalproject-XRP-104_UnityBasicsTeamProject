using System.Collections;
using UnityEngine;

/// <summary>
/// 게임 전체 흐름을 관리하는 중앙 관리자
/// - Player 생성
/// - 체크포인트 관리
/// - 리스폰 처리
/// - UI에서 Player 접근 가능하도록 제공
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("플레이어 프리팹")]
    [SerializeField] private PlayerController _playerPrefab;

    private PlayerController _spawnedPlayer;

    private Vector3 _currentCheckpointPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // SpawnPoint를 직접 찾아 위치 설정 (실행 순서 의존 제거)
        SpawnPoint spawn = FindAnyObjectByType<SpawnPoint>();

        if (spawn != null)
        {
            _currentCheckpointPosition = spawn.transform.position;
        }
        else
        {
            Debug.LogError("SpawnPoint가 씬에 없습니다.");
        }

        SpawnPlayer();
    }

    /// <summary>
    /// 런타임 Player 생성
    /// </summary>
    private void SpawnPlayer()
    {
        _spawnedPlayer = Instantiate(
            _playerPrefab,
            _currentCheckpointPosition,
            Quaternion.identity
        );
    }

    /// <summary>
    /// UI에서 현재 생성된 Player 접근용
    /// </summary>
    public PlayerController GetPlayer()
    {
        return _spawnedPlayer;
    }

    /// <summary>
    /// 체크포인트 저장
    /// </summary>
    public void SetCheckpoint(Vector3 position)
    {
        _currentCheckpointPosition = position;
    }

    /// <summary>
    /// 플레이어 리스폰
    /// </summary>
    public void RespawnPlayer()
    {
        if (_spawnedPlayer == null) return;

        _spawnedPlayer.transform.position =
            _currentCheckpointPosition + Vector3.up * 2f;

        PlayerHealth health = _spawnedPlayer.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.ResetHealth();
            health.StartCoroutine(
                health.RespawnInvincibleRoutine(1.5f));
        }
    }
}