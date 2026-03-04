using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 오버 UI 관리
/// - 런타임 생성 Player에 이벤트 구독
/// - 사망 시 패널 표시
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("게임오버 패널")]
    [SerializeField] private GameObject _panel;

    private PlayerHealth _playerHealth;

    private void Start()
    {
        // 시작 시 패널 숨김
        _panel.SetActive(false);

        // GameManager 통해 런타임 생성 Player 가져오기
        PlayerController player = GameManager.Instance.GetPlayer();

        if (player != null)
        {
            _playerHealth = player.GetComponent<PlayerHealth>();

            if (_playerHealth != null)
            {
                _playerHealth.OnDeath += ShowGameOver;
            }
        }
        else
        {
            Debug.LogWarning("Player가 아직 생성되지 않음");
        }
    }

    /// <summary>
    /// 플레이어 사망 시 호출
    /// </summary>
    private void ShowGameOver()
    {
        Debug.Log("Game Over UI 표시");

        _panel.SetActive(true);

        // 시간 정지
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Retry 버튼
    /// </summary>
    public void OnClickRetry()
    {
        _panel.SetActive(false);

        GameManager.Instance.RespawnPlayer();

        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath -= ShowGameOver;
        }
    }
}