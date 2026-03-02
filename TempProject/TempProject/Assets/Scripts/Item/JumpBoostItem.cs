using UnityEngine;

public class JumpBoostItem : ItemBase
{
    protected override void ApplyEffect(PlayerController player)
    {
        Debug.Log("Jump Boost È°¼ºÈ­");
        player.EnableJumpBoost();
    }
}