using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

namespace Assets.GameAssets.Zombies
{
    public class DeadZombieState : BaseCharacterState3D
    {
        private ZombieController zombie;

        public DeadZombieState(ZombieController zombie)
        {
            this.zombie = zombie;
        }

        public override void EnterState()
        {
            zombie.Brain.Disabled();
            zombie.Agent.Disabled();
            zombie.Animator.SetTrigger(ZombieAnimParams.Dead);

            zombie.GetComponent<CapsuleCollider>().enabled = false;
        }

        public override void TriggerAnimationEvent(string eventName)
        {
            //if(eventName == "is_dead")
            //    zombie.InstantiateRagdoll();
        }
    }
}