using Assets.UnityFoundation.Systems.HealthSystem;

namespace Assets.GameAssets.Items
{
    public class HealItem : ConsumableItem
    {
        public float HealAmount { get; private set; }

        private IHealable healable;

        public HealItem(float healAmount)
        {
            HealAmount = healAmount;
        }

        public HealItem SetHealable(IHealable healable)
        {
            this.healable = healable;
            return this;
        }

        protected override void OnUse()
        {
            healable.Heal(HealAmount);
        }
    }
}