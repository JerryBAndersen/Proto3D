using System;
using UnityEngine;

namespace Proto {
    public class Food : Visible, IPotable
    {
        public Food(string name, Vector3 position, Quaternion rotation, GameObject prefab, EntityManager manager) : base(name, position, rotation, prefab, manager)
        {
        }

        public int nutritionalValue
        {
            get
            {
                return 10;
            }
        }

        public void Consume()
        {
            Destroy();
        }
    }
}