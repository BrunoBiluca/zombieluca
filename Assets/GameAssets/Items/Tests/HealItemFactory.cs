using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class HealItemFactory : IConsumableItemFactory
    {
        private HealItem item;

        private readonly List<HealItemMonoBehaviour> list;

        public HealItemFactory()
        {
            list = new List<HealItemMonoBehaviour>();
        }

        public ICollisionObject Create()
        {
            item = new HealItem(10);

            var i = new GameObject("heal")
                .AddComponent<HealItemMonoBehaviour>()
                .Setup(item);

            list.Add(i);

            return i;
        }

        public void Destroy()
        {
            foreach (var i in list)
                try
                {
                    Object.DestroyImmediate(i.gameObject);
                }
                catch(MissingReferenceException)
                {
                    continue;
                }
        }

        public ConsumableItem GetConsumableItem()
        {
            return item;
        }
    }
}