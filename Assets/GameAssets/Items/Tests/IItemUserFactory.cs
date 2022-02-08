using UnityEngine;

namespace Assets.GameAssets.Items.Tests
{
    public interface IItemUserFactory
    {
        public MonoBehaviour Create();
    }
}