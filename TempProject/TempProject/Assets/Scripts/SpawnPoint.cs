using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.SetCheckpoint(transform.position);
    }

    private void Start()
    {
        // 게임 시작 시 기본 체크포인트 위치 저장
        GameManager.Instance.SetCheckpoint(transform.position);

        Debug.Log("기본 시작 위치 저장 완료");
    }
}
