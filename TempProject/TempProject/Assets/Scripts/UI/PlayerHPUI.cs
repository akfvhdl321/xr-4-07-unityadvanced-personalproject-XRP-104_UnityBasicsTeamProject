using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Image[] _hearts;

    private void Update()
    {
        if (_playerHealth == null) return;

        int current = _playerHealth.CurrentHP;

        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].enabled = i < current;
        }
    }
}