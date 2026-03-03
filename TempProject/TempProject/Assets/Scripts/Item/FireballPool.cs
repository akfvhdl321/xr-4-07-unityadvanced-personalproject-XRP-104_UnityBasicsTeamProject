using System.Collections.Generic;
using UnityEngine;

public class FireballPool : MonoBehaviour
{
    [Header("프리팹")]
    [Tooltip("Fireball 프리팹 연결")]
    [SerializeField] private Fireball _fireballPrefab;

    [Header("초기 풀 개수")]
    [Tooltip("미리 생성할 개수")]
    [SerializeField] private int _poolSize = 5;

    [Header("동시 활성 제한")]
    [Tooltip("화면 내 최대 Fireball 개수")]
    [SerializeField] private int _maxActiveCount = 10;

    private Queue<Fireball> _pool = new Queue<Fireball>();
    private int _currentActiveCount;

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewFireball();
        }
    }

    private void CreateNewFireball()
    {
        Fireball fb = Instantiate(_fireballPrefab);
        fb.SetPool(this);
        fb.gameObject.SetActive(false);
        _pool.Enqueue(fb);
    }

    public bool CanSpawn()
    {
        return _currentActiveCount < _maxActiveCount;
    }

    public Fireball GetFireball()
    {
        Debug.Log("현재 ActiveCount: " + _currentActiveCount);
        Debug.Log("현재 PoolCount: " + _pool.Count);

        if (!CanSpawn())
        {
            Debug.Log("CanSpawn 실패");
            return null;
        }

        if (_pool.Count == 0)
        {
            Debug.Log("Pool 비어있음 → 새로 생성");
            CreateNewFireball();
        }

        Fireball fb = _pool.Dequeue();
        fb.gameObject.SetActive(true);

        _currentActiveCount++;

        return fb;
    }

    public void ReturnFireball(Fireball fb)
    {
        fb.gameObject.SetActive(false);
        _pool.Enqueue(fb);

        _currentActiveCount--;
    }
}
