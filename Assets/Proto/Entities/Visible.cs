using System;
using UnityEngine;

namespace Proto {
    public class Visible : Entity
    {
        public GameObject prefab;
        public GameObject go;
        public Vector3 position;
        public Quaternion rotation;
        public Visible(string name, Vector3 position, Quaternion rotation, GameObject prefab, EntityManager manager) : base(name,manager) {
            this.prefab = prefab;
            this.position = position;
            this.rotation = rotation;
        }
        public Visible(Visible entity) : base(entity.name,entity.manager){
            this.prefab = entity.prefab;
            this.position = entity.position;
            this.rotation = entity.rotation;
        }

        public override void Update(float delta){
            if(go) {
                go.transform.position = position;
                go.transform.rotation = rotation;
            }
        }
    }
}