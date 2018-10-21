using System;
using System.Collections.Generic;
using UnityEngine;

namespace Proto {
    public class Hero : Visible, IMortal
    {
        private int _food = 100;
        private int _health = 100;

        public Hero(string name, Vector3 position, Quaternion rotation, GameObject prefab, EntityManager manager) : base(name, position, rotation, prefab, manager)
        {
            
        }

        public void Consume(IPotable potable){
            potable.Consume();
            _food = Math.Min(100,Math.Max(0,_food + potable.nutritionalValue));
        }

        public void Die(){
            ChangeInto((Dead) this);
        }
    }
}