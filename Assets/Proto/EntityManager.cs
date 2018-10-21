using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Proto {
    public class EntityManager {        
        public List<Entity> entities;   
        private List<KeyValuePair<Entity,Entity>> changes; 
        private List<Entity> removals;
        private List<Entity> additions;
        public IObserver[] observers;

        public EntityManager(){
            entities = new List<Entity>();
            changes = new List<KeyValuePair<Entity, Entity>>();
            removals = new List<Entity>();
            additions = new List<Entity>();
            maxUpdateMs = App.instance.config.maxUpdateMs;
            maxSqrDistance = App.instance.config.maxDistance * App.instance.config.maxDistance;
        }
        public EntityManager(List<Entity> entities) : this() {
            Add(entities);
        }

        public void Update() {   
            ProcessUpdates();
            ProcessChanges();
            ProcessRemovals();
            ProcessAdditions();
        }

        public int entityIterator;
        private float maxSqrDistance;
        private float maxUpdateMs;
        private int visibleCount = 0;
        private void ProcessUpdates(){
            if(entityIterator >= entities.Count){
                entityIterator = 0;
            }   
            for(; entityIterator < entities.Count; entityIterator++) {
                Entity entity = entities[entityIterator];
                long milliDelta = App.instance.gameTime.ElapsedMilliseconds-entity.stamp;
                entity.Update(milliDelta/1000f);
                entity.stamp = App.instance.gameTime.ElapsedMilliseconds;
                if(entity is Visible) {
                    Visible visible = entity as Visible;
                    foreach(IObserver observer in observers) {
                        bool isVisible = Vector3.SqrMagnitude(observer.position - visible.position) < maxSqrDistance;
                        if(isVisible){
                            if(visibleCount < App.instance.config.maxEntities){
                                if(!visible.go) {
                                    visibleCount++;
                                    visible.go = GameObject.Instantiate(visible.prefab);
                                    visible.go.name = visible.name;
                                    visible.Update(0f);
                                }
                            }else{
                                App.print("visibleCount > maxEntities!");
                            }
                        }else{
                            if(visible.go){
                                if(visibleCount > 0){
                                    visibleCount--;
                                }                                
                                GameObject.Destroy(visible.go);
                            }
                        }
                    }
                }
                if(App.instance.updateTime.ElapsedMilliseconds > maxUpdateMs) {
                    break;
                }
            }
        }

        public void ChangeInto(Entity entity, Entity into)
        {
            if(removals.Contains(entity)){
                throw new Exception("Can't change destroyed Entity!");
            }
            changes.Add(new KeyValuePair<Entity, Entity>(entity,into));
        }

        public void Add(Entity entity)
        {
            additions.Add(entity);
        }

        public void Add(List<Entity> entities)
        {
            additions.AddRange(entities);
        }

        public void Remove(Entity entity)
        {          
            // remove entity from changes
            for(int i = 0; i < changes.Count; i++){
                var change = changes[i];
                if(change.Key == entity){
                    changes.RemoveAt(i);
                    break;
                }
            }
            removals.Add(entity);
        }

        private void ProcessChanges(){
            foreach(KeyValuePair<Entity, Entity> change in changes) {
                Remove(change.Key);
                Add(change.Value);
            }
            changes.Clear();
        }
        private void ProcessRemovals(){
            foreach(Entity removal in removals) {
                entities.Remove(removal);
            }           
            removals.Clear();
        }
        private void ProcessAdditions(){
            entities.AddRange(additions);
            additions.Clear();
        }
    }
}