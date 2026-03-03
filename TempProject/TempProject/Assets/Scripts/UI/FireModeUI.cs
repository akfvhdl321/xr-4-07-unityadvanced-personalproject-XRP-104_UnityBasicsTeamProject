using UnityEngine;
using UnityEngine.UI;

public class FireModeUI : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Image _icon;

    private void Update()
    {
        if (_player == null) return;

        _icon.enabled = _player.IsFireModeActive;
    }
}