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

    public int CurrentAmount => storage.CurrentAmount;

    public int MaxAmount => storage.MaxAmount;

    public void Recover(int amount) => storage.Recover(amount);
}
