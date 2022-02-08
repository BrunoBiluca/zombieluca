using Assets.GameAssets.AmmoStorage;
using Assets.UnityFoundation.UnityAdapter;
using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Items
{
    public class AmmoItemMonoBehaviour : MonoBehaviour, ICollisionObject
    {
        [SerializeField] private AudioClip pickupAmmoSFX;

        private AmmoItem ammoItem;
        private IAudioSource audioS;

        [Inject]
        public AmmoItemMonoBehaviour Setup(
            AmmoItem ammoItem,
            IAudioSource audio
        )
        {
            this.ammoItem = ammoItem;
            this.audioS = audio;
            return this;
        }

        public void OnCollisionEnter(Collision collision)
        {
            if(!collision.gameObject.TryGetComponent(out IAmmoStorage ammoStorage))
                return;

            ammoItem
                .SetAmmoStorage(ammoStorage)
                .Use();

            audioS.PlayOneShot(pickupAmmoSFX);
            Destroy(gameObject);
        }
    }
}