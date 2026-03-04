using UnityEngine;
using TMPro;

/// <summary>
/// JumpBoost 남은 시간 표시 UI
/// </summary>
public class JumpBoostUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private PlayerController _player;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        if (_player == null) return;

        if (_player.IsJumpBoostActive)
        {
            float remain = _player.JumpBoostRemainingTime;
            _text.text = "점프 부스트 시간: " + remain.ToString("F1") + "s";
            _text.gameObject.SetActive(true);
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }
}