using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLockHandler
{
    private readonly FirstPersonInputActions inputsActions;

    public CursorLockHandler(FirstPersonInputActions inputsActions)
    {
        this.inputsActions = inputsActions;
    }

    public void Enable()
    {
        inputsActions.Cursor.Lock.performed += OnLock;
        inputsActions.Cursor.Release.performed += OnRelease;

        inputsActions.Cursor.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnLock(InputAction.CallbackContext ctx)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnRelease(InputAction.CallbackContext ctx)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
