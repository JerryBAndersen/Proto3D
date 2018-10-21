using UnityEngine;
namespace Proto {
    public class Player : MonoBehaviour, IObserver
    {
        public Vector3 position
        {
            get
            {
                return transform.position;
            }
        }
    }
}