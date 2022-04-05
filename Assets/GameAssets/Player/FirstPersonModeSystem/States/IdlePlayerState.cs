using Assets.GameAssets.Player;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

namespace Assets.GameAssets.FirstPersonModeSystem
{
    public class IdlePlayerState : BaseCharacterState3D
    {
        private readonly FirstPersonMode controller;
        private readonly FirstPersonAnimationController animController;

        public IdlePlayerState(FirstPersonMode controller)
        {
            this.controller = controller;
        }

        public override void EnterState()
        {
            controller.AnimController.Walking(false);
        }

        public override void Update()
        {
            controller.Rotate();
            controller.Move();
        }
    }
}