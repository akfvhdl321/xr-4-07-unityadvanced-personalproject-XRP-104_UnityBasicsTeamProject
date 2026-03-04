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
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("GameManager Awake 실행");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*private void Start()
    {
        InitializeSpawnPoint();
        SpawnPlayer();
    }*/

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded 호출됨: " + scene.name);
        // Stage 씬에서만 Player 생성
        if (scene.name == "Stage 1" || scene.name == "Stage 2")
        {
            InitializeSpawnPoint();
            SpawnPlayer();
        }
    }

    private void InitializeSpawnPoint()
    {
        // 현재 씬의 SpawnPoint를 항상 다시 찾음
        SpawnPoint spawn = FindAnyObjectByType<SpawnPoint>();

        if (spawn != null)
        {
            _currentCheckpointPosition = spawn.transform.position;
            Debug.Log("Spawn 위치 설정됨: " + _currentCheckpointPosition);
        }
        else
        {
            Debug.LogError("SpawnPoint가 씬에 없음");
        }
    }



    private void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer 호출됨");
        if (_playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab 연결 안됨");
            return;
        }

        _spawnedPlayer = Instantiate(
            _playerPrefab,
            _currentCheckpointPosition,
            Quaternion.identity
        );
        Debug.Log("Player 생성 완료");

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