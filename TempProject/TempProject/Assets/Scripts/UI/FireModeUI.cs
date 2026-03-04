using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FireMode 활성 여부 표시 UI
/// </summary>
public class FireModeUI : MonoBehaviour
{
    [SerializeField] private Image _icon;

    private PlayerController _player;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        if (_player == null) return;

        _icon.enabled = _player.IsFireModeActive;
    }
}