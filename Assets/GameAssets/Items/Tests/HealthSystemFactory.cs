using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public class HealthSystemFactory : IItemUserFactory
    {
        public MonoBehaviour Create()
        {
            return new GameObject("healable", typeof(HealthSystem))
                .GetComponent<HealthSystem>();
        }
    }
}