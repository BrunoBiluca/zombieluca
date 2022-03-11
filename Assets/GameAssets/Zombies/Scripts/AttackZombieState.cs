using Assets.UnityFoundation.Systems.Character3D.Scripts;

namespace Assets.GameAssets.Zombies
{
    public class AttackZombieState : BaseCharacterState3D
    {
        public class TriggerEvents
        {
            public static readonly string AttackFinished = "attack_finished";
        }

        private ZombieController zombie;
        private bool canExit;

        public AttackZombieState(ZombieController zombie)
        {
            this.zombie = zombie;
        }

        public override bool CanBeInterrupted => canExit;

        public override void EnterState()
        {
            canExit = false;
            zombie.Animator.SetTrigger(ZombiesAnimParams.Attack);
        }

        public override void TriggerAnimationEvent(string eventName)
        {
            if(TriggerEvents.AttackFinished.Equals(eventName))
            {
                canExit = true;
                zombie.TransitionToState(zombie.IdleState);
            }

        }
    }
}