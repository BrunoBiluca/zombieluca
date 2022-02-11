using Assets.GameAssets.AmmoStorage;
using UnityEngine;
using Zenject;

public class AmmoStorageMonoBehaviour : MonoBehaviour, IAmmoStorage
{
    private IAmmoStorage storage;

    [Inject]
    public AmmoStorageMonoBehaviour Setup(IAmmoStorage storage)
    {
        this.storage = storage;
        return this;
    }

    public uint CurrentAmount => storage.CurrentAmount;

    public uint MaxAmount => storage.MaxAmount;

    public void Recover(uint amount) => storage.Recover(amount);
}
