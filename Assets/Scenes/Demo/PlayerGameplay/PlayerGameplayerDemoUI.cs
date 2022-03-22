using Assets.GameAssets.AmmoStorageSystem;
using Assets.GameAssets.Player;
using Assets.UnityFoundation.Systems.HealthSystem;
using TMPro;
using UnityEngine;
using UnityFoundation.Code;
using Zenject;

public class PlayerGameplayerDemoUI : MonoBehaviour
{
    [Inject]
    private FirstPersonController player;
    private HealthSystem healthSystem;
    private AmmoStorageMonoBehaviour ammoStorage; 

    private TextMeshProUGUI healthText;
    private TextMeshProUGUI ammoText;

    public void Awake()
    {
        healthText = transform.FindComponent<TextMeshProUGUI>("player_stats.health_text");
        ammoText = transform.FindComponent<TextMeshProUGUI>("player_stats.ammo_storage_text");
    }

    public void Start()
    {
        healthSystem = player.GetComponent<HealthSystem>();
        ammoStorage = player.GetComponent<AmmoStorageMonoBehaviour>();
    }

    public void Update()
    {
        healthText.text = $"Health: {healthSystem.CurrentHealth}";
        ammoText.text = $"Ammo: {ammoStorage.CurrentAmount}";
    }
}
