using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.AmmoStorageSystem;
using Zenject;

public class AmmoStorageBinder : AmmoStorageMonoBehaviour
{
    [Inject]
    public void InjectEntry(IAmmoStorage ammoStorage)
    {
        Setup(ammoStorage);
    }
}
