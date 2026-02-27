using UnityEngine;

public class IdleState : IPlayerState
{
    PlayerController _player;

    public IdleState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (_player._moveInput != 0)
        {
            _player.ChangeState(_player.Move);
        }
    }
}
