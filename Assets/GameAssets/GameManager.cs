using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    private readonly CursorLockHandler cursorLockHandler;

    public GameManager(CursorLockHandler cursorLockHandler) {
        this.cursorLockHandler = cursorLockHandler;
    }

    public void Initialize()
    {
        cursorLockHandler.Enable();
    }
}
