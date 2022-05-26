using UnityFoundation.Compass;
using Zenject;

public class CompassTrackedObjectBinder : CompassTrackedObject
{
    [Inject]
    public void Init(CompassView compassView)
    {
        CompassView = compassView;
    }
}
