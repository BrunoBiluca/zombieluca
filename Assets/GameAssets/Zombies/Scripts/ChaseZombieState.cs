using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

namespace Assets.GameAssets.Zombies
{
    public class ChaseZombieState : BaseCharacterState3D
    {
        private readonly ZombieController zombie;

        public ChaseZombieState(ZombieController zombie)
        {
            this.zombie = zombie;
        }

        public override void EnterState()
        {
            zombie.Animator.SetBool(ZombiesAnimParams.Walking, zombie.Brain.IsWalking);
            zombie.Animator.SetBool(ZombiesAnimParams.Running, zombie.Brain.IsRunning);
            zombie.Agent.Speed = zombie.Config.ChasingSpeed;
        }

        public override void Update()
        {
            if(zombie.Brain.IsAttacking)
            {
                zombie.TransitionToState(zombie.AttackState);
                return;
            }

            zombie.Brain.TargetPosition
                .Some((target) => SetupDestination(target));
    
            if(!zombie.Brain.IsChasing)
                zombie.TransitionToState(zombie.IdleState);
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