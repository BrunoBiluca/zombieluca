using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.Zombies;
using Zenject;

namespace Assets.GameAssets.Zombies
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public partial class ZombieController : BaseCharacter3D
    {
        public Settings Config { get; private set; }

        public IAnimator Animator { get; private set; }
        public IAIBrain Brain { get; private set; }
        public INavMeshAgent Agent { get; private set; }
        public AudioSourceDec AudioSource { get; private set; }

        public IdleZombieState IdleState { get; private set; }
        public WanderZombieState WanderState { get; private set; }
        public ChaseZombieState ChaseState { get; private set; }
        public AttackZombieState AttackState { get; private set; }
        public DeadZombieState DeadState { get; private set; }
        public IHasHealth Health { get; private set; }
        public GameObject PlayerRef { get; private set; }

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
            Brain.Enabled();

            Config = config;

            Agent = agent;

            AudioSource = new AudioSourceDec(GetComponent<AudioSource>());

            IdleState = new IdleZombieState(this);
            WanderState = new WanderZombieState(this);
            ChaseState = new ChaseZombieState(this);
            AttackState = new AttackZombieState(this);
            DeadState = new DeadZombieState(this);

            Health = hasHealth;
            hasHealth.Setup(config.BaseHealth);
            hasHealth.OnDied += (sender, args) => TransitionToStateForce(DeadState);

            TransitionToState(IdleState);
            return this;
        }

        public void SetPlayerRef(GameObject player)
        {
            PlayerRef = player;
            Brain.SetPlayer(player);
        }

        protected override void OnUpdate()
        {
            Brain.Update();

            if(Brain.IsChasing)
                TransitionToStateIfDifferent(ChaseState);
        }
    }
}