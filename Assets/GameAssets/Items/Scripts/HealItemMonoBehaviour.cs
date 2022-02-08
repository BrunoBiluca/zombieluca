using Assets.GameAssets.Items;
using Assets.UnityFoundation.Systems.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Items
{
    public class HealItemMonoBehaviour : MonoBehaviour, ICollisionObject
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

            Destroy(gameObject);
        }
    }
}