using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioClip _bgm;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_bgm != null)
        {
            _audioSource.clip = _bgm;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}