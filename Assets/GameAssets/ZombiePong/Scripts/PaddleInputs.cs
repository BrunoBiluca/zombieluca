using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public class PaddleInputs
    {
        public Vector2 Move => inputActions.Player.Move.ReadValue<Vector2>();

        private readonly PongInputActions inputActions;

        public PaddleInputs(PongInputActions inputActions)
        {
            this.inputActions = inputActions;
        }

        public void Enable()
        {
            inputActions.Enable();
        }
    }
}