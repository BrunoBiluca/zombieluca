﻿namespace Assets.GameAssets.Zombies
{
    public interface IDecisionHandler<TContext>
    {
        IDecisionHandler<TContext> SetNext(IDecisionHandler<TContext> decisionHandler);
        IDecisionHandler<TContext> SetFailed(IDecisionHandler<TContext> decisionHandler);
        void Handle(TContext context);
    }
}