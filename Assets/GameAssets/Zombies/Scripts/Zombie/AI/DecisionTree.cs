namespace Assets.GameAssets.Zombies
{
    public abstract class DecisionTree<TContext>
    {
        private IDecisionHandler<TContext> rootHandler;

        protected void SetRootHandler(IDecisionHandler<TContext> handler)
        {
            rootHandler = handler;
        }

        public void Update(TContext context)
        {
            if(rootHandler != null)
                rootHandler.Handle(context);
        }
    }
}