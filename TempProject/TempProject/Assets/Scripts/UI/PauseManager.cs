using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        _pausePanel.SetActive(true);
        Time.timeScale = 0f;   // 게임 정지
        _isPaused = true;
    }

    public void ResumeGame()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;   // 게임 재개
        _isPaused = false;
    }

    public void GoToTitle()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title Scene");
    }
}