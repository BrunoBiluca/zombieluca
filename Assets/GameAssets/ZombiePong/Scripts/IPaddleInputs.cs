using UnityEngine;

namespace Assets.GameAssets.ZombiePong
{
    public interface IPaddleInputs
    {
        Vector2 Move { get; }
        void Enable();
    }
}