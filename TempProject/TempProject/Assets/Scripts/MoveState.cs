using UnityEngine;

public class MoveState : IPlayerState
{
    PlayerController _player;

    public MoveState (PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player._animator.SetBool("IsMove",true);
       
    }

    public void Update()
    {
        if(_player.CanJump())
        {
            _player.ConsumeJump();
            _player.ChangeState(_player.Jump);
            return;
        }

        if(_player._moveInput == 0)
        {
            _player.ChangeState(_player.Idle);
        }

    }

    public void Exit()
    {
        
    }

    
}
