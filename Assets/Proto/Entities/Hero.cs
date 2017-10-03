using UnityEngine;

namespace Proto.Entities {
    public class Hero : Entity {
        public Vector3 target;
        public string name;

        public Hero(string name) {
            this.name = name;
        }
    }
}
