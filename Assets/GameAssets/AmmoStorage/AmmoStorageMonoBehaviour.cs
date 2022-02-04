using Assets.GameAssets.AmmoStorage;
using UnityEngine;
using Zenject;

public class AmmoStorageMonoBehaviour : MonoBehaviour, IAmmoStorage
{
    [Inject]
    private readonly IAmmoStorage ammoStorage;

    public int CurrentAmount => ammoStorage.CurrentAmount;

    public int MaxAmount => ammoStorage.MaxAmount;

    public void Recover(int amount) => ammoStorage.Recover(amount);
}
