using UnityEngine;

public class FireModeItem : ItemBase
{
    protected override void ApplyEffect(PlayerController player)
    {
        Debug.Log("FireModeItem Trigger °¨Áö");
        player.EnableFireMode();
    }
}