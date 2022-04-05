using Assets.GameAssets.FirstPersonModeSystem;
using UnityEngine;
using Zenject;

[CreateAssetMenu(
    fileName = "new_player_settings_installer", 
    menuName = "Player/Player Settings Installer"
)]
public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
{
    public FirstPersonModeSettings firstPersonModeSettings;
    public ZombilucaPlayerSettings playerSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(firstPersonModeSettings);
        Container.BindInstance(playerSettings);
    }
}