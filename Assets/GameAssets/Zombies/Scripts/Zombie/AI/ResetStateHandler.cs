namespace Assets.GameAssets.Zombies
{
    public class ResetStateHandler : BaseDecisionHandler<SimpleBrainContext>
    {
        public override bool OnHandle(SimpleBrainContext context)
        {
            context.ResetStates();
            return true;
        }
    }
}