using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.GameAssets.Player.Tests
{
    public class PlayerStatesTest
    {
        [Test, Ignore("Not implemented")]
        public void ShouldEnterOnWalkingState_WhenMoveFromIdleState()
        {
            var player = new Mock<FirstPersonController>();

            var inputs = new Mock<FirstPersonInputs>();

            var idleState = new IdlePlayerState(
                player.Object,
                inputs.Object,
                new Mock<FirstPersonAnimationController>().Object,
                new Mock<Camera>().Object
            );

            idleState.Update();
        }
    }
}