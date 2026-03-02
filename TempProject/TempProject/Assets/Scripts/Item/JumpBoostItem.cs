using UnityEngine;

public class JumpBoostItem : ItemBase
{
    protected override void ApplyEffect(PlayerController player)
    {
        player.EnableJumpBoost();
    }
}