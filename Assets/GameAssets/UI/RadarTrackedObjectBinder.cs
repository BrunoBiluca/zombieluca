using UnityFoundation.Radar;
using Zenject;

public class RadarTrackedObjectBinder : RadarTrackedObject
{
    [Inject]
    public void Setup(RadarView radarView)
    {
        RadarView = radarView;
    }
}
