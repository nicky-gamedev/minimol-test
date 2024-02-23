using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerActions.IGameplayActions
{
    private PlayerActions _playerInput;
    public event Action JumpEvent = delegate {};

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _playerInput ??= new PlayerActions();
        _playerInput.Gameplay.SetCallbacks(this);
        EnableInput();
    }
    
    public void EnableInput()
    {
        _playerInput.Gameplay.Enable();    
    }

    public void DisableInput()
    {
        _playerInput.Gameplay.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpEvent();
    }
}
