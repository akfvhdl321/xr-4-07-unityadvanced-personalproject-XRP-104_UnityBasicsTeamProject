using UnityEngine;

public class PlayerStateMachine
{
    public IPlayerState _currentState;

    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();

    }

    public void Update()
    {
        _currentState?.Update();
    }
}
