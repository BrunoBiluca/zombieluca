using Assets.GameAssets.Zombies;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

public class WanderZombieState : BaseCharacterState3D
{
    private readonly ZombieController zombie;

    private readonly Transform targetWandering;

    public WanderZombieState(ZombieController zombie)
    {
        this.zombie = zombie;
        targetWandering = new GameObject("target_wandering").transform;
    }

    public override void EnterState()
    {
        zombie.Animator.SetBool(ZombiesAnimParams.Walking, zombie.Brain.IsWalking);
        zombie.Animator.SetBool(ZombiesAnimParams.Running, zombie.Brain.IsRunning);
    }

    public override void Update()
    {
        zombie.Brain.TargetPosition
            .Some((target) => {
                zombie.Agent.SetDestination(target);
                targetWandering.position = target;
            });
    }
}
