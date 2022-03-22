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

        public override bool ForceInterruption => true;

        public override void EnterState()
        {
            zombie.Brain.Disabled();
            zombie.Agent.Disabled();
            zombie.GetComponent<CapsuleCollider>().enabled = false;

            var change = Random.Range(0f, 1f);
            if(change > .5f)
                zombie.Animator.SetTrigger(ZombieAnimParams.Dead);
            else
                zombie.InstantiateRagdoll();
        }
    }
}