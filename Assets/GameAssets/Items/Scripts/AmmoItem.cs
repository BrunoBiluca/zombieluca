using Assets.GameAssets.AmmoStorage;

namespace Assets.GameAssets.Items
{
    public class AmmoItem : ConsumableItem
    {
        public int RefillAmount { get; private set; }

        private IAmmoStorage ammoStorage;

        public AmmoItem(int refillAmount)
        {
            RefillAmount = refillAmount;
        }

        public AmmoItem SetAmmoStorage(IAmmoStorage ammoStorage)
        {
            this.ammoStorage = ammoStorage;
            return this;
        }

        protected override void OnUse()
        {
            ammoStorage.Recover(RefillAmount);
        }
    }
}