using Assets.GameAssets.Player;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;
using Zenject;

namespace Assets.GameAssets.UI
{
    public class PlayerUI : MonoBehaviour
    {
        private Image hitShotImage;
        private TextMeshProUGUI ammoCount;
        private ZombilucaPlayer player;

        public void Start()
        {
            hitShotImage = transform.FindComponent<Image>("hit_shot_image");

            ammoCount = transform.FindComponent<TextMeshProUGUI>("ammo_count_view", "value");
        }

        [Inject]
        public void Setup(
            SignalBus signalBus,
            ZombilucaPlayer player
        )
        {
            this.player = player;
            signalBus.Subscribe<PlayerHitShotSignal>(() => ShowHitShotImage());
        }

        public void Update()
        {
            ammoCount.text = player.AmmoStorage.CurrentAmount.ToString();
        }

        private void ShowHitShotImage()
        {
            hitShotImage.gameObject.SetActive(true);
            StartCoroutine(FadeHitShotImage());
        }

        private IEnumerator FadeHitShotImage()
        {
            yield return new WaitForSeconds(.5f);
            hitShotImage.gameObject.SetActive(false);
        }
    }
}