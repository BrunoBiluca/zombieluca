using Assets.UnityFoundation.Systems.Character3D.Scripts;

namespace Assets.GameAssets.Zombies
{
    public class IdleZombieState : BaseCharacterState3D
    {
        private readonly ZombieController zombie;

        public IdleZombieState(
            ZombieController zombie
        )
        {
            this.zombie = zombie;
        }

        public override void EnterState()
        {
            zombie.Animator.SetBool(ZombieAnimParams.Walking, false);
            zombie.Animator.SetBool(ZombieAnimParams.Running, false);
        }

        public override void Update()
        {
            if(zombie.Brain.IsWandering)
                zombie.TransitionToState(zombie.WanderState);
        }
    }
}