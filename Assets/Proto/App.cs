using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Proto {
    public class App : MonoBehaviour {

        public StateMachine<App> stateMachine;
        public static App instance;        
        public Stopwatch gameTime;
        public Stopwatch updateTime;

        [Serializable]
        public class GameConfig {
            public Player[] players;
            public int entityCount { get {
                    return heroConfig.heroCount;
                } }
            public int maxEntities = 100;
            public float worldSize = 100;
            public int maxUpdateMs = 5;
            public float maxDistance = 100f;
            [Serializable]
            public class DebugConfig {
                public bool isDebug = false;
            }
            public DebugConfig debugConfig;
            [Serializable]
            public class HeroConfig {
                public int foodRatePerHour = 100/(3 * 7 * 24)<=0? 1:100/(3 * 7 * 24);
                public GameObject heroPrefab;
                public int heroCount = 100;
            }
            public HeroConfig heroConfig;
            [Serializable]
            public class NameConfig {
                public string[] firstnames = { "Hans", "Peter", "Knut", "Tim", "Tom", "Karl", "Benedikt" };
                public string[] lastnames = { "Schneider", "Meier", "Schuhmacher", "Hansen", "Jensen", "Sörensen"};

                public string GetName(int i) {
                    i = i%(firstnames.Length*firstnames.Length*lastnames.Length);
                    int firstname = 0;
                    int middlename = 0;
                    int lastname = 0;
                    while(i > 0) {
                        firstname++;
                        if(firstname >= firstnames.Length) {
                            firstname = 0;
                            middlename++;
                            if(middlename >= firstnames.Length) {
                                middlename = 0;
                                lastname++;
                                if(lastname >= lastnames.Length) {
                                    lastname = 0;
                                }
                            }
                        }
                        i--;
                    }
                    return firstnames[firstname] + " " + firstnames[middlename] + " " + lastnames[lastname]; 
                }
            }
            public NameConfig nameConfig;            
        }
        public GameConfig config;

        [Serializable]
        public class State : StateMachine<App>.State
        {
            public State(App r) : base(r)
            {
            }
        }
        [Serializable]
        public class InGame : State {

            public InGame(App r) : base(r)
            {
            }

            EntityManager manager;
            public override void Enter() {
                List<Entity> entities;
                if (Directory.Exists("data") && !instance.config.debugConfig.isDebug) {
                    var paths = Directory.GetFiles("data");
                    entities = new List<Entity>();
                    for(int i = 0; i < paths.Length; i++) {
                        string path = paths[i];
                        string file = File.ReadAllText(path);
                        entities.Add(JsonUtility.FromJson<Entity>(file));
                    }
                } else {
                    entities = new List<Entity>(instance.config.entityCount);
                    // create heroes
                    for(int i = 0; i < instance.config.heroConfig.heroCount; i++) {
                        NPCHero h = new NPCHero(randomName,randomPosition,Quaternion.identity,instance.config.heroConfig.heroPrefab,manager);
                        entities.Add(h);
                    }
                }
                manager = new EntityManager(entities);
                manager.observers = instance.config.players;

                instance.gameTime = new Stopwatch();
                instance.updateTime = new Stopwatch();
                instance.gameTime.Start();
            }

            public Vector3 randomPosition { 
                get {
                    float worldSize = instance.config.worldSize;
                    return new Vector3(worldSize/2-worldSize*UnityEngine.Random.value,0, worldSize/2-worldSize*UnityEngine.Random.value);
                } 
            }

            public string randomName { get{
                return instance.config.nameConfig.GetName(new System.Random().Next());
            } }

            public override void Update() {   
                instance.updateTime.Stop();
                instance.updateTime.Reset();
                instance.updateTime.Start();
                manager.Update();
            }

            public override void Exit() {
                if (Directory.Exists("data")) {
                    Directory.Delete("data");
                }
                Directory.CreateDirectory("data");
                for (int i = 0; i < manager.entities.Count; i++) {
                    string path = manager.entities[i].GetHashCode().ToString();
                    string json = JsonUtility.ToJson(manager.entities[i]);
                    File.WriteAllText(path, json);
                }
            }
        }

        public InGame inGameState;

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else {
                throw new System.Exception("Only one instance allowed!");
            }
            if (File.Exists("config") && !config.debugConfig.isDebug) {
                string file = File.ReadAllText("config");
                config = JsonUtility.FromJson<GameConfig>(file);
            }

            inGameState.Init(this);
            stateMachine = new StateMachine<App>(inGameState);
        }

        private void Start() {
        }

        private void Update() {
            stateMachine.Update();
        }

        private void SaveConfig() {
            if (File.Exists("config")) {
                File.Delete("");
            }
            if (!File.Exists("config")) {
                string file = File.ReadAllText("config");
                config = JsonUtility.FromJson<GameConfig>(file);
            }
        }
    }
}
