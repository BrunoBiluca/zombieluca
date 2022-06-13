using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;
using Zenject;

namespace Assets.GameAssets.Items
{
    public class HealItemMonoBehaviour : BilucaMonoBehaviour, ICollisionObject
    {
        private HealItem healItem;

        [Inject]
        public HealItemMonoBehaviour Setup(HealItem healItem)
        {
            this.healItem = healItem;
            return this;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if(!collision.gameObject.TryGetComponent(out IHealable healable))
                return;

            healItem.SetHealable(healable).Use();

            if(healItem.WasConsumed)
#if UNITY_EDITOR
                DestroyImmediate(gameObject);
#else
                Destroy(gameObject);
#endif
        }
    }
}