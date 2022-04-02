namespace Assets.GameAssets.Items
{
    public abstract class ConsumableItem
    {
        public bool WasConsumed { get; private set; }

        public ConsumableItem() { }

        public void Use()
        {
            if(WasConsumed) return;

            if(!IsValidToUse()) return;

            WasConsumed = true;
            OnUse();
        }

        protected virtual bool IsValidToUse() { return true; }

        protected abstract void OnUse();

        // TODO: verificar a disponibilidade de implementar uma factory com todos as implementações de ConsumableItem
    }
}