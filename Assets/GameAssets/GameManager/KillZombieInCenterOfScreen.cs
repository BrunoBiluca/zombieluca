using Assets.GameAssets.Zombies;
using Assets.UnityFoundation.CameraScripts;
using Assets.UnityFoundation.Systems.HealthSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityFoundation.Code;

public class KillZombieInCenterOfScreen : MonoBehaviour
{
    void Update()
    {
        if(!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        var ray = Camera.main.ScreenPointToRay(CameraUtils.ScreenCenter());
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            var zombie = hit.transform.GetComponent<ZombieController>();

            if(zombie != null)
                zombie.GetComponent<HealthSystem>().Damage(10f);
        }
    }
}
