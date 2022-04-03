using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

namespace Assets.GameAssets.Zombies
{
    public class WanderZombieState : BaseCharacterState3D
    {
        private readonly ZombieController zombie;

        private readonly Transform targetWandering;

        public WanderZombieState(ZombieController zombie)
        {
            this.zombie = zombie;
            targetWandering = new GameObject("target_wandering").transform;
        }

        public override void EnterState()
        {
            zombie.Animator.SetBool(ZombieAnimParams.Walking, zombie.Brain.IsWalking);
            zombie.Animator.SetBool(ZombieAnimParams.Running, zombie.Brain.IsRunning);

            zombie.Agent.StoppingDistance = 0f;
            zombie.Agent.Speed = zombie.Config.WanderingSpeed;

            zombie.AudioSource.Volume = 0.2f;
            zombie.AudioSource.Loop = true;
            zombie.AudioSource.Play(zombie.Config.WanderingSFX);
        }

        public override void ExitState()
        {
            zombie.AudioSource.ResetAudio();
        }

        public override void Update()
        {
            zombie.Brain.TargetPosition
                .Some((target) => {
                    zombie.Agent.SetDestination(target);
                    targetWandering.position = target;
                })
                .OrElse(() => {
                    zombie.Agent.ResetPath();
                    zombie.TransitionToState(zombie.IdleState);
                });
        }
    }
}