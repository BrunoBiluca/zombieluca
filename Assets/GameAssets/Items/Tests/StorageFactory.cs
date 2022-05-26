using Moq;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.AmmoStorageSystem;

namespace Assets.GameAssets.Items.Tests
{
    public class StorageFactory : IItemUserFactory
    {

        public List<AmmoStorageMonoBehaviour> list;
        private bool startFull;

        public StorageFactory()
        {
            startFull = false;
            list = new List<AmmoStorageMonoBehaviour>();
        }
        
        public StorageFactory Full()
        {
            startFull = true;
            return this;
        }

        public MonoBehaviour Create()
        {
            var ammoStorage = new AmmoStorage(5, startFull);

            var ammo = new GameObject("storage")
                .AddComponent<AmmoStorageMonoBehaviour>()
                .Setup(ammoStorage);

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