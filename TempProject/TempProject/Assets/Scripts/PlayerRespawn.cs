using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Respawn(Vector3 position)
    {
        Debug.Log("리스폰 위치 이동: " +  position);

        _rb.linearVelocity = Vector2.zero;

        transform.position = position;
    }
}
