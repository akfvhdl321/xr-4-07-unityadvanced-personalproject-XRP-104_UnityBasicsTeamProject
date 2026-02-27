using UnityEngine;

public class JumpState : IPlayerState
{
    PlayerController _player;

    private bool _isFalling;

    public JumpState(PlayerController player)
    {
        _player = player;
    }
    public void Enter()
    {
        _player._rb.linearVelocity = new Vector2(_player._rb.linearVelocity.x, _player._jumpForce);

    }

    public void Update()
    {
        
        if (_player._isGrounded)
        {
            if(_player._moveInput == 0)
            {
                _player.ChangeState(_player.Idle);
            }
            else
            {
                _player.ChangeState(_player.Move);
            }
        }
    }

    public void Exit()
    {
        
    }

    
}
