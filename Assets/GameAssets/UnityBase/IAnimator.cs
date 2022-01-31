namespace Assets.GameAssets.UnityBase
{
    public interface IAnimator
    {
        void SetTrigger(string name);
        void SetBool(string name, bool value);
        bool GetBool(string name);
    }
}