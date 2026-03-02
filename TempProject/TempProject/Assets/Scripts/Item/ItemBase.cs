using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            ApplyEffect(player);
            gameObject.SetActive(false);
        }
    }

    // 각 아이템별 효과 구현
    protected abstract void ApplyEffect(PlayerController player);
}