using System;
using System.Collections.Generic;
using UnityEngine;

namespace Proto {
    public class NPCHero : Hero
    {
        public List<Entity> visibleEntities;

        private enum Priority {
            Food = 0,
            Survival = 1
        }
        public int[] priorities = new int[Enum.GetNames(typeof(Priority)).Length];

        Vector3 initPosition;

        public NPCHero(string name, Vector3 position, Quaternion rotation, GameObject prefab, EntityManager manager) : base(name, position, rotation, prefab, manager)
        {
            initPosition = position;
        }

        private int GetPriority(Priority p){
            return priorities[(int)p];
        }
        private void SetPriority(Priority priority, int value){
            value = Math.Min(100,Math.Max(0,value));
            priorities[(int)priority] = value;
        }

        float time = 0f;
        public override void Update(float delta)
        {
            base.Update(delta);

            time += delta;
            position = initPosition + new Vector3(Mathf.Sin(time),0,Mathf.Cos(time));

            // get top priority
            Priority topPriority = Priority.Survival;
            int v = -1;
            for(int i = 0; i < priorities.Length; i++){
                if(v < priorities[i]){
                    topPriority = (Priority) i;
                    v = priorities[i];
                }                
            }

            switch(topPriority){
                case Priority.Food:{
                    break;
                }
                case Priority.Survival:{
                    break;
                }
            }
        }
    }
}