using Assets.UnityFoundation.UnityAdapter;
using Moq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class AmmoItemFactory : IConsumableItemFactory
    {
        private AmmoItem item;

        private readonly List<AmmoItemMonoBehaviour> list;

        public AmmoItemFactory()
        {
            list = new List<AmmoItemMonoBehaviour>();
        }

        public ICollisionObject Create()
        {
            item = new AmmoItem(5);

            var obj = new GameObject("ammo")
                .AddComponent<AmmoItemMonoBehaviour>()
                .Setup(item, new Mock<IAudioSource>().Object);

            list.Add(obj);

            return obj;
        }

        public void Destroy()
        {
            foreach(var item in list)
            {
                try
                {
                    Object.DestroyImmediate(item.gameObject);
                }
                catch(MissingReferenceException)
                {
                    continue;
                }
            }
        }

        public ConsumableItem GetConsumableItem()
        {
            return item;
        }
    }
}