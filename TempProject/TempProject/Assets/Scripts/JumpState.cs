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
        Debug.Log("점프 동작함");
        _player._animator.SetBool("IsJump", true);
        _player._animator.SetBool("IsMove", false);
        _player._rb.linearVelocity = new Vector2(_player._rb.linearVelocity.x, 0);

        _player._rb.linearVelocity = new Vector2(_player._rb.linearVelocity.x, _player._jumpForce);
    }

    public void Update()
    {
        Debug.Log("Ground 상태 : " + _player._isGrounded);

        if (_player._rb.linearVelocity.y > 0) return;

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
        _player._animator.SetBool("IsJump", false);
    }

    
}
