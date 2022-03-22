using Assets.GameAssets.Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;
using Zenject;

public class PlayerUI : MonoBehaviour
{
    private Image hitShotImage;

    public void Start()
    {
        hitShotImage = transform.FindComponent<Image>("hit_shot_image");
    }

    [Inject]
    public void Setup(SignalBus signalBus)
    {
        signalBus.Subscribe<HitShotSignal>(() => ShowHitShotImage());
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
