using Proto.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Proto {
    public class GameController : Controller {

        public static GameController instance;        

        [Serializable]
        public class GameConfig {
            public int maxTicksForUpdate = 1;
            public int entityCount { get {
                    return heroCount;
                } }
            public int heroCount = 200000;
            public int maxEntities = 100;
            [Serializable]
            public class NameConfig {
                public string[] firstnames = { "Hans", "Peter", "Knut", "Tim", "Tom", "Karl", "Benedikt" };
                public string[] lastnames = { "Schneider", "Meier", "Schuhmacher", "Hansen", "Jensen", "Sörensen"};

                public string GetName(int i) {
                    int firstname = 0;
                    int middlename = 0;
                    int lastname = 0;
                    while(i > 0) {
                        firstname++;
                        if(firstname > firstnames.Length) {
                            firstname = 0;
                            middlename++;
                            if(middlename > firstnames.Length) {
                                lastname++;
                                if(lastname > lastnames.Length) {
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

        public class GameState : State {
            public class InGame : GameState {
                GameObject[] entitiesGOs;
                Entity[] entities;
                Hashtable hashEntities;
                int entityIterator = 0;
                int stamp = 0;

                public override void Enter() {
                    if (Directory.Exists("data")) {
                        var paths = Directory.GetFiles("data");
                        entities = new Entity[paths.Length];
                        for(int i = 0; i < paths.Length; i++) {
                            string path = paths[i];
                            string file = File.ReadAllText(path);
                            JsonUtility.FromJson<Entity>(file);
                        }
                    } else {
                        entities = new Entity[instance.config.entityCount];
                        for(int i = 0; i < entities.Length; i++) {
                            entities[i] = new Hero(instance.config.nameConfig.GetName(i));
                            entities[i].position = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value);
                        }
                    }
                }

                public override void Update() {
                    for(; entityIterator < entities.Length; entityIterator++) {
                        entities[entityIterator].Update();
                        if(Environment.TickCount - stamp > instance.config.maxTicksForUpdate) {

                        }
                    }                    
                }

                public override void Exit() {
                    if (Directory.Exists("data")) {
                        Directory.Delete("data");
                    }
                    Directory.CreateDirectory("data");
                    for (int i = 0; i < entities.Length; i++) {
                        string path = entities[i].GetHashCode().ToString();
                        string json = JsonUtility.ToJson(entities[i]);
                        File.WriteAllText(path, json);
                    }
                }
            }
        }

        private GameState.InGame inGameState;

        private void Awake() {
            if(instance == null) {
                instance = this;
            } else {
                throw new System.Exception("Only one instance allowed!");
            }
            if (File.Exists("config")) {
                string file = File.ReadAllText("config");
                config = JsonUtility.FromJson<GameConfig>(file);
            }

            inGameState = new GameState.InGame();
        }

        private void Start() {
            SetState(inGameState);
        }

        private void Update() {
            state.Update();
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
