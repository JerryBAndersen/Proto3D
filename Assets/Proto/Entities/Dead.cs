using System;
using UnityEngine;

namespace Proto {
    public class Dead : Visible
    {
        public Dead(string name, Vector3 position, Quaternion rotation, GameObject prefab, EntityManager manager) : base(name, position, rotation, prefab, manager)
        {
        }

        public Dead(Hero hero) : base(hero)
        {
        }

        public static explicit operator Dead(Hero hero)
        {
            return new Dead(hero);
        }
    }
}