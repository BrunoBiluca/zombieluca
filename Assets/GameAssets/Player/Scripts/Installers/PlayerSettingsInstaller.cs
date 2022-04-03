using Assets.GameAssets.Player;
using UnityEngine;
using Zenject;

[CreateAssetMenu(
    fileName = "new_player_settings_installer", 
    menuName = "Player/Player Settings Installer"
)]
public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
{
    public PlayerSettings settings;

    public override void InstallBindings()
    {
        Container.BindInstance(settings);
    }
}