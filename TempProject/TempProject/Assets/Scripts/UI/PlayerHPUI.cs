using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 체력 UI
/// GameManager를 통해 런타임 생성 Player 참조
/// </summary>
public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;

    private PlayerHealth _playerHealth;

    private void Start()
    {
        // 런타임 생성 Player 가져오기
        PlayerController player = GameManager.Instance.GetPlayer();

        if (player != null)
        {
            _playerHealth = player.GetComponent<PlayerHealth>();
        }

        if (_playerHealth != null)
        {
            UpdateUI();
        }
    }

    private void Update()
    {
        if (_playerHealth == null) return;

        UpdateUI();
    }

    private void UpdateUI()
    {
        int current = _playerHealth.CurrentHP;

        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = i < current;
        }
    }
}