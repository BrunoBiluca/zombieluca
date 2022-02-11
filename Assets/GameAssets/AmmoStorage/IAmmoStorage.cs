namespace Assets.GameAssets.AmmoStorage
{
    public interface IAmmoStorage
    {
        uint CurrentAmount { get; }

        uint MaxAmount { get; }

        void Recover(uint amount);
    }
}