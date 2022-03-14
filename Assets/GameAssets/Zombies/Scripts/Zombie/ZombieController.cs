using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using Assets.UnityFoundation.UnityAdapter;
using UnityEngine;
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
        public AttackZombieState AttackState { get; private set; }

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
            Brain.DebugMode = config.DebugMode;
            Config = config;

            Agent = agent;

            IdleState = new IdleZombieState(this);
            WanderState = new WanderZombieState(this);
            ChaseState = new ChaseZombieState(this);
            AttackState = new AttackZombieState(this);

            hasHealth.OnDied += (sender, args) => Animator.SetTrigger(ZombieAnimParams.Dead);

            TransitionToState(IdleState);
            return this;
        }
        public void SetPlayerRef(GameObject player)
        {
            Brain.SetPlayer(player);
        }

        protected override void OnUpdate()
        {
            Brain.Update();

            if(Brain.IsChasing)
                TransitionToStateIfDifferent(ChaseState);
        }

        public class Settings
        {
            public float WanderingSpeed;
            public float ChasingSpeed;
            public float ChasingTurnSpeed;

            public bool DebugMode;
        }
    }
}