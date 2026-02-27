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
        
    }

    public void Update()
    {
        if(_player._moveInput == 0)
        {
            _player.ChangeState(_player.Idle);
        }
    }

    public void Exit()
    {
        
    }

    
}
