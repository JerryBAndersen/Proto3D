using System;
using UnityEngine;

namespace Proto
{
    public class Entity {
        public string name;
        public string guid {get; private set;}
        public long stamp;
        public EntityManager manager;
        public Entity(string name, EntityManager manager) {
            this.guid = Guid.NewGuid().ToString();
            this.name = name;
            this.manager = manager;
        }
        public Entity(Entity entity) {
            this.guid = Guid.NewGuid().ToString();
            this.name = entity.name;
            this.manager = entity.manager;
        }

        public virtual void Update(float delta) { }

        protected void ChangeInto(Entity result){
            manager.ChangeInto(this, result);
        }

        public void Destroy(){
            manager.Remove(this);
        }
    }
}