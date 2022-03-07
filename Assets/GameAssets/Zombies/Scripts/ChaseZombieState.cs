using Assets.UnityFoundation.Systems.Character3D.Scripts;

namespace Assets.GameAssets.Zombies
{
    public class ChaseZombieState : BaseCharacterState3D
    {
        private ZombieController zombieController;

        public ChaseZombieState(ZombieController zombieController)
        {
            this.zombieController = zombieController;
        }
    }
}