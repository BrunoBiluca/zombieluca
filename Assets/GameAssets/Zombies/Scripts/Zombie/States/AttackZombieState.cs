using Assets.UnityFoundation.Systems.Character3D.Scripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using UnityFoundation.Code;

namespace Assets.GameAssets.Zombies
{
    public class AttackZombieState : BaseCharacterState3D
    {
        public class TriggerEvents
        {
            public static readonly string Attack = "attack";
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
            zombie.Animator.SetBool(ZombieAnimParams.Attack, true);
        }

        public override void ExitState()
        {
            zombie.Animator.SetBool(ZombieAnimParams.Attack, false);
        }

        public override void TriggerAnimationEvent(string eventName)
        {
            if(TriggerEvents.Attack.Equals(eventName))
            {
                zombie.Brain.Target.Some(target => {
                    if(target.Distance(zombie.transform) > zombie.Config.AttackRange)
                        return;

                    if(zombie.Config.AttackSFX != null)
                    {
                        var randSound = Random.Range(0, zombie.Config.AttackSFX.Length);
                        zombie.AudioSource.PlayOneShot(zombie.Config.AttackSFX[randSound]);
                    }
                    target.GetComponent<IDamageable>().Damage(zombie.Config.AttackDamage);
                });
                return;
            }

            if(TriggerEvents.AttackFinished.Equals(eventName))
            {
                canExit = true;
                zombie.TransitionToState(zombie.IdleState);
            }
        }
    }
}