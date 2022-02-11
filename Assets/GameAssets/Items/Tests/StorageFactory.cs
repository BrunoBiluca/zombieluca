using Assets.GameAssets.AmmoStorage;
using Moq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class StorageFactory : IItemUserFactory
    {

        public List<AmmoStorageMonoBehaviour> list;

        public StorageFactory()
        {
            list = new List<AmmoStorageMonoBehaviour>();
        }

        public MonoBehaviour Create()
        {
            var ammo = new GameObject("storage")
                .AddComponent<AmmoStorageMonoBehaviour>()
                .Setup(new Mock<IAmmoStorage>().Object);

            list.Add(ammo);

            return ammo;
        }

        public void Destroy()
        {
            foreach(var ammo in list)
                try
                {
                    Object.DestroyImmediate(ammo.gameObject);
                }
                catch(MissingReferenceException)
                {
                    continue;
                }
        }
    }
}