using UnityEngine;

public class FireModelItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            player.EnableFireMode();
            gameObject.SetActive(false);
        }
    }
}
