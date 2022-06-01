using Assets.GameAssets.Player;
using Assets.UnityFoundation.Systems.HealthSystem;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;
using UnityFoundation.UI;
using Zenject;

namespace Assets.GameAssets.UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private BloodSplatter bloodSplatterPrefab;

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
            player.GetComponent<IHasHealth>().OnTakeDamage += OnPlayerTakeDamage;
            signalBus.Subscribe<PlayerHitShotSignal>(() => ShowHitShotImage());
        }

        private void OnPlayerTakeDamage(object sender, EventArgs e)
        {
            Instantiate(bloodSplatterPrefab, transform)
                .Setup(2f, GetComponent<RectTransform>().rect.size);
            Instantiate(bloodSplatterPrefab, transform)
                .Setup(2f, GetComponent<RectTransform>().rect.size);
            Instantiate(bloodSplatterPrefab, transform)
                .Setup(2f, GetComponent<RectTransform>().rect.size);
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