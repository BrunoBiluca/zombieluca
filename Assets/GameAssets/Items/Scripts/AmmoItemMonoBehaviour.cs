using Assets.GameAssets.AmmoStorage;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Items
{
    public class AmmoItemMonoBehaviour : MonoBehaviour
    {
        [Inject]
        private readonly AmmoItem ammoItem;

        private void OnCollisionEnter(Collision collision)
        {
            if(!collision.gameObject.TryGetComponent(out IAmmoStorage ammoStorage))
                return;

            ammoItem
                .SetAmmoStorage(ammoStorage)
                .Use();

            Destroy(gameObject);
        }
    }
}