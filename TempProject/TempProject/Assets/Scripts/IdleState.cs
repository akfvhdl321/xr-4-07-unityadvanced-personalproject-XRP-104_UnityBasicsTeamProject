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
        _player._animator.SetBool("IsMove", false);
        _player._animator.SetBool("IsJump", false);
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
