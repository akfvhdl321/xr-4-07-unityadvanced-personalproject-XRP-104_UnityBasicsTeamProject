using UnityEngine;
using UnityEngine.SceneManagement;

public class StageGoal : MonoBehaviour
{
    [Header("¥Ÿ¿Ω æ¿ ¿Ã∏ß")]
    [SerializeField] private string _nextSceneName = "Stage 2";

    private bool _isCleared;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCleared) return;

        if (other.TryGetComponent<PlayerController>(out var player))
        {
            _isCleared = true;

            Debug.Log("Stage Clear!");

            LoadNextStage();
        }
    }

    private void LoadNextStage()
    {
        SceneManager.LoadScene(_nextSceneName);
    }
}