using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;   // 게임오버 패널
    [SerializeField] private PlayerHealth _playerHealth;

    private void Start()
    {
        // 시작 시 패널 숨김
        _panel.SetActive(false);

        // 플레이어 사망 이벤트 구독
        _playerHealth.OnDeath += ShowGameOver;
    }

    private void ShowGameOver()
    {
        Debug.Log("Game Over UI 표시");

        _panel.SetActive(true);

        // 플레이어 조작 정지
        Time.timeScale = 0f;
    }

    // Retry 버튼에서 호출
    public void OnClickRetry()
    {
        Debug.Log("Retry 클릭");

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
            _playerHealth.OnDeath -= ShowGameOver;
    }
}