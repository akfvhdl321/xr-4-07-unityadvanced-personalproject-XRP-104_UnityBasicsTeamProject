using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool _isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player 태그인지 확인
        if (_isActivated)
            return;

        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        _isActivated = true;

        Debug.Log("체크포인트 활성화");

        GameManager.Instance.SetCheckpoint(transform.position);

        // 나중에 이펙트나 색상 변경 추가 가능
    }
}
