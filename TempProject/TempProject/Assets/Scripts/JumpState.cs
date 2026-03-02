using UnityEngine;

public class JumpState : IPlayerState
{
    PlayerController _player;

    public JumpState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        
        _player.PerformJump();
    }

    public void Update()
    {
        if (_player._isGrounded)
        {
            if (_player._moveInput == 0)
                _player.ChangeState(_player.Idle);
            else
                _player.ChangeState(_player.Move);
        }
    }

    public void Exit()
    {
    }
}