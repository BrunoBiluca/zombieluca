using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Radar
{
    public class RadarTrackedObject : MonoBehaviour
    {
        [Inject] private readonly RadarView radar;

        [field: SerializeField] private GameObject objRefPrefab;

        public void Start()
        {
            radar.Register(transform, objRefPrefab);
        }

        public void UnRegister()
        {
            radar.UnRegister(transform);
        }

        public void OnDestroy()
        {
            radar.UnRegister(transform);
        }
    }
}