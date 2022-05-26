using UnityFoundation.AmmoStorageSystem;

namespace Assets.GameAssets.Items
{
    public class AmmoItem : ConsumableItem
    {
        public uint RefillAmount { get; private set; }

        private IAmmoStorage ammoStorage;

        public AmmoItem(uint refillAmount)
        {
            RefillAmount = refillAmount;
        }

        public AmmoItem SetAmmoStorage(IAmmoStorage ammoStorage)
        {
            this.ammoStorage = ammoStorage;
            return this;
        }

        protected override bool IsValidToUse()
        {
            return !ammoStorage.IsFull;
        }

        protected override void OnUse()
        {
            ammoStorage.Recover(RefillAmount);
        }
    }
}