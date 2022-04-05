using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;
using UnityFoundation.Code.TimeUtils;

namespace Assets.GameAssets.Zombies
{
    public class ChaseZombieState : BaseCharacterState3D
    {
        private readonly ZombieController zombie;
        private Timer updateAudioTimer;

        public ChaseZombieState(ZombieController zombie)
        {
            this.zombie = zombie;
            UpdateChaseAudio();
            updateAudioTimer = new Timer(0.4f, UpdateChaseAudio).Loop();
        }

        private void UpdateChaseAudio()
        {
            if(zombie.Config.ChasingSFX == null) return;

            var clipIdx = Random.Range(0, zombie.Config.ChasingSFX.Length - 1);
            zombie.AudioSource.Loop = true;
            zombie.AudioSource.Play(zombie.Config.ChasingSFX[clipIdx]);
        }

        public override void EnterState()
        {
            zombie.Animator.SetBool(ZombieAnimParams.Walking, zombie.Brain.IsWalking);
            zombie.Animator.SetBool(ZombieAnimParams.Running, zombie.Brain.IsRunning);
            
            zombie.Agent.Speed = zombie.Config.ChasingSpeed;
        }

        public override void Update()
        {
            if(zombie.Brain.IsAttacking)
            {
                zombie.TransitionToState(zombie.AttackState);
                return;
            }

            if(!zombie.Brain.IsChasing)
            {
                zombie.TransitionToState(zombie.IdleState);
                return;
            }
            
            zombie.Brain.TargetPosition
                .Some((target) => SetupDestination(target))
                .OrElse(() => zombie.Agent.ResetPath());
        }

        public override void ExitState()
        {
            zombie.Agent.ResetPath();
            zombie.AudioSource.ResetAudio();
        }

        private void SetupDestination(Vector3 target)
        {
            zombie.Agent.SetDestination(target);

            if((target - zombie.transform.position).magnitude < 0.1f)
                return;

            var direction = (target - zombie.transform.position).normalized;
            var qDir = Quaternion.LookRotation(direction);
            zombie.transform.rotation = Quaternion.Slerp(
                zombie.transform.rotation, qDir, Time.deltaTime * zombie.Config.ChasingTurnSpeed
            );
        }
    }
}