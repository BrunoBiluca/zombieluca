using UnityEngine;

namespace Assets.GameAssets.Items
{
    public interface ICollisionObject
    {
        void OnCollisionEnter(Collision collision);
    }
}