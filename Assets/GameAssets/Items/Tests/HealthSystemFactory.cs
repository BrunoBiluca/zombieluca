using Assets.UnityFoundation.Systems.HealthSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class HealthSystemFactory : IItemUserFactory
    {
        private readonly List<HealthSystem> healthSystems;
        private bool isFull;

        public HealthSystemFactory()
        {
            isFull = false;
            healthSystems = new List<HealthSystem>();
        }

        public MonoBehaviour Create()
        {
            var hs = new GameObject("healable", typeof(HealthSystem))
                .GetComponent<HealthSystem>();

            if(isFull)
                hs.HealFull();
            else
                hs.Damage(5);

            healthSystems.Add(hs);

            return hs;
        }

        public void Destroy()
        {
            foreach (var hs in healthSystems)
                try
                {
                    Object.DestroyImmediate(hs.gameObject);
                }
                catch(MissingReferenceException)
                {
                    continue;
                }
        }

        public HealthSystemFactory Full()
        {
            isFull = true;
            return this;
        }
    }
}