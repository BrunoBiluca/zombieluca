using Zenject;

namespace Assets.GameAssets.Items
{
    public abstract class ConsumableItem
    {
        public bool WasConsumed { get; private set; }

        public void Use()
        {
            if(WasConsumed) return;

            WasConsumed = true;
            OnUse();
        }

        protected abstract void OnUse();

        // TODO: verificar a disponibilidade de implementar uma factory com todos as implementações de ConsumableItem
    }
}