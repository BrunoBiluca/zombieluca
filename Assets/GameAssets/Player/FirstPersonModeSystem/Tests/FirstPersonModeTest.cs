using Assets.GameAssets.FirstPersonModeSystem;
using Assets.UnityFoundation.UnityAdapter;
using Moq;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityFoundation.Code.PhysicsUtils;

namespace Assets.GameAssets.Player.FirstPersonModeSystem.Tests
{
    public class FirstPersonModeTest
    {
        private void BuildFirstPersonModePlayer(
            out FirstPersonMode firstPerson, 
            out Mock<ICheckGroundHandler> checkGroundMock,
            out Mock<IRigidbody> rigidBodyMock)
        {
            firstPerson = new GameObject("player").AddComponent<FirstPersonMode>();

            checkGroundMock = new Mock<ICheckGroundHandler>();
            var inputActionsMock = new Mock<FirstPersonInputActions>();

            var inputsMock = new Mock<IFirstPersonInputs>();
            inputsMock.SetupGet(x => x.Jump).Returns(true);

            firstPerson.Setup(
                ScriptableObject.CreateInstance<FirstPersonModeSettings>(),
                inputsMock.Object,
                checkGroundMock.Object,
                animController: null,
                audioSource: new Mock<IAudioSource>().Object,
                camera: null
            );

            rigidBodyMock = new Mock<IRigidbody>();
            firstPerson.Rigidbody = rigidBodyMock.Object;
        }

        [Test]
        public void ShouldNotJumpWhenWasNotOnGround()
        {
            BuildFirstPersonModePlayer(
                out FirstPersonMode firstPerson, 
                out Mock<ICheckGroundHandler> checkGroundMock,
                out Mock<IRigidbody> rigidBodyMock
            );

            checkGroundMock.SetupGet(cg => cg.IsGrounded).Returns(false);

            firstPerson.TryJump();

            rigidBodyMock.Verify(
                rb => rb.AddForce(It.IsAny<Vector3>(), It.IsAny<ForceMode>()),
                Times.Never
            );
        }

        [Test]
        public void ShouldJumpWhenWasOnGround()
        {
            BuildFirstPersonModePlayer(
                out FirstPersonMode firstPerson, 
                out Mock<ICheckGroundHandler> checkGroundMock,
                out Mock<IRigidbody> rigidBodyMock
            );

            checkGroundMock.SetupGet(cg => cg.IsGrounded).Returns(true);

            firstPerson.TryJump();

            checkGroundMock.SetupGet(cg => cg.IsGrounded).Returns(false);

            firstPerson.TryJump();
            firstPerson.TryJump();

            rigidBodyMock.Verify(
                rb => rb.AddForce(It.IsAny<Vector3>(), It.IsAny<ForceMode>()),
                Times.Once
            );
        }
    }
}