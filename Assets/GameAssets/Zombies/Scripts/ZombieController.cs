using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
using Zenject;

namespace Assets.GameAssets.Zombies
{
    public class ZombieController : BaseCharacter3D
    {
        public Settings Config { get; private set; }

        public IAnimator Animator { get; private set; }
        public IAIBrain Brain { get; private set; }
        public INavMeshAgent Agent { get; private set; }

        public IdleZombieState IdleState { get; private set; }
        public WanderZombieState WanderState { get; private set; }
        public ChaseZombieState ChaseState { get; private set; }

        [Inject]
        public ZombieController Setup(
            Settings config,
            IAnimator anim,
            IAIBrain brain,
            IHasHealth hasHealth,
            INavMeshAgent agent
        )
        {
            Animator = anim;
            Brain = brain;
            Config = config;

            Agent = agent;
            Agent.Speed = Config.Speed;

            IdleState = new IdleZombieState(this);
            WanderState = new WanderZombieState(this);
            ChaseState = new ChaseZombieState(this);

            hasHealth.OnDied += (sender, args) => Animator.SetTrigger(ZombiesAnimParams.Dead);

            TransitionToState(IdleState);
            return this;
        }

        protected override void OnUpdate()
        {
            Brain.Update();

            if(Brain.IsChasing)
                TransitionToState(ChaseState);

            if(Brain.IsAttacking)
            {
                Animator.SetTrigger(ZombiesAnimParams.Attack);
            }
        }

        public class Settings
        {
            public float Speed;
        }
    }
}