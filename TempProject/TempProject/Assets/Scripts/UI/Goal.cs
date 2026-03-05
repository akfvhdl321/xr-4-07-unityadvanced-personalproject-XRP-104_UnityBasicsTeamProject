using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject _clearPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("스테이지 클리어");

        _clearPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}