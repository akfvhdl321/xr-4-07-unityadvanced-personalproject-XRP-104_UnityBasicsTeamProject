using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

/// <summary>
/// 게임 전체 관리자
/// - Player 생성
/// - 체크포인트 저장
/// - 씬 리로드 시 체크포인트 유지
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
            DontDestroyOnLoad(gameObject); //씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 씬 로드될 때 Player 다시 생성
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        InitializeSpawnPoint();
        SpawnPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeSpawnPoint();
        SpawnPlayer();
    }

    private void InitializeSpawnPoint()
    {
        // 체크포인트가 아직 없으면 기본 SpawnPoint 위치 사용
        if (_currentCheckpointPosition == Vector3.zero)
        {
            SpawnPoint spawn = FindAnyObjectByType<SpawnPoint>();

            if (spawn != null)
            {
                _currentCheckpointPosition = spawn.transform.position;
            }
        }
    }

    private void SpawnPlayer()
    {
        if (_playerPrefab == null) return;

        _spawnedPlayer = Instantiate(
            _playerPrefab,
            _currentCheckpointPosition,
            Quaternion.identity
        );

        // Cinemachine 연결
        CinemachineCamera cam = FindAnyObjectByType<CinemachineCamera>();
        if (cam != null)
        {
            cam.Follow = _spawnedPlayer.transform;
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        _currentCheckpointPosition = position;
    }

    public bool HasCheckpoint()
    {
        return _currentCheckpointPosition != Vector3.zero;
    }

    public PlayerController GetPlayer()
    {
        return _spawnedPlayer;
    }
}