using UnityEngine;
using TMPro;

public class JumpBoostUI : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        if (_player == null) return;

        if (_player.IsJumpBoostActive)
        {
            float remain = _player.JumpBoostRemainingTime;
            _text.text = "Jump Boost: " + remain.ToString("F1") + "s";
            _text.gameObject.SetActive(true);
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }
}