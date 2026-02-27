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
        _player._animator.SetBool("IsJump", false);
    }

    public void Update()
    {
        if(_player._moveInput == 0)
        {
            _player.ChangeState(_player.Idle);
        }

        if (_player._moveInput > 0)
        {
            _player.transform.localScale = new Vector3(3, 3, 3);
        }

        else if ( _player._moveInput < 0)
        {
            _player.transform.localScale = new Vector3(-3, 3, 3);
        }
    }

    public void Exit()
    {
        
    }

    
}
