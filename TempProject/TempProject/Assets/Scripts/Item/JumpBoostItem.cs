using UnityEngine;

public class JumpBoostItem : ItemBase
{
    protected override void ApplyEffect(PlayerController player)
    {
        Debug.Log("JumpBoostItem ApplyEffect »£√‚µ ");
        player.EnableJumpBoost();
    }
}