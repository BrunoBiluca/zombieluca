using Assets.UnityFoundation.Code.Characters;
using Assets.UnityFoundation.Systems.Character3D.Scripts;
using UnityEngine;

public class Character3DAnimationEventProxy : MonoBehaviour
{
    private BaseCharacter3D baseCharacter;

    private void Start() {
        baseCharacter = GetComponentInParent<BaseCharacter3D>();
        Debug.Log(baseCharacter);
    }

    public void TriggerAnimationEvent(string name)
    {
        baseCharacter.TriggerAnimationEvent(name);
    }
}
