using System;

namespace Assets.GameAssets.AmmoStorage
{
    public class AmmoStorage : IAmmoStorage
    {
        private int currentAmount;
        public int CurrentAmount => currentAmount;

        private readonly int maxAmount;
        public int MaxAmount => maxAmount;

        public AmmoStorage(int maxAmount)
        {
            this.maxAmount = maxAmount;
        }

        public void Recover(int amount)
        {
            currentAmount += amount;
            currentAmount = Math.Max(currentAmount, maxAmount);
        }
    }
}