using UnityEngine;
using Zenject;

namespace Assets.GameAssets.Compass
{
    public class CompassTrackedObject : MonoBehaviour
    {
        [Inject]
        private readonly CompassView compass;

        public void Start()
        {
            compass.Controller.Register(transform);
        }
    }
}