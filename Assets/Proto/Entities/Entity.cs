using System;
using UnityEngine;

namespace Proto.Entities {
    public class Entity {
        private static int idSource = 0;
        public int id;
        public Vector3 position;

        public Entity() {
            this.id = idSource++;
        }

        public virtual void Update() {
            
        }
    }
}
