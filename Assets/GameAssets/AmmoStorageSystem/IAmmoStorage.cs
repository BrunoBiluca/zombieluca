namespace Assets.GameAssets.AmmoStorageSystem
{
    public interface IAmmoStorage
    {
        uint CurrentAmount { get; }

        uint MaxAmount { get; }

        void FullReffil();
        uint GetAmmo(uint amount);
        void Recover(uint amount);
    }
}