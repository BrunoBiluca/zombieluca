namespace Assets.GameAssets.AmmoStorage
{
    public interface IAmmoStorage
    {
        int CurrentAmount { get; }

        int MaxAmount { get; }

        void Recover(int amount);
    }
}