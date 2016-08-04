﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Platformer {
    public class World {
        public Player player;
        public TileSpriteSheet TileSpriteSheet { get; private set; }
        public Dictionary<Point, Tile> Tiles { get; private set; }
        public List<Entity> Entities { get; private set; }
        List<Entity> entitiesToRemove;
        public List<Particle> Particles { get; private set; }
        List<Particle> particlesToRemove;
        public GameTime gameTime;
        public Random rand;
        public Dictionary<string, string> StoryVars;
        public Dialogue DialogueBox;

        public World(TileSpriteSheet tileSpriteSheet) {
            TileSpriteSheet = tileSpriteSheet;
            Tiles = new Dictionary<Point, Tile>();
            Entities = new List<Entity>();
            entitiesToRemove = new List<Entity>();
            Particles = new List<Particle>();
            particlesToRemove = new List<Particle>();
            rand = new Random(); //Should seed
            StoryVars = new Dictionary<string, string>();
        }

        private void ParseSavedEntity(string line) {
            string[] symbols = line.Split(',');
            Entity entity = null;

            switch (symbols[0]) {
                case "Coin":
                    var newCoin = new Coin(this);
                    newCoin.Pos = new Vector2(Convert.ToInt32(symbols[1]), Convert.ToInt32(symbols[2]));
                    entity = newCoin;
                    break;
                default:
                    System.Console.WriteLine("No Such Entity");
                    break;
            }
            Entities.Add(entity);
        }

        public Player LoadLevel(string path) {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes[1].ChildNodes) {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "tile") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value);
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value);
                    int tx = Convert.ToInt32(xmlNode.Attributes["tx"].Value);
                    int ty = Convert.ToInt32(xmlNode.Attributes["ty"].Value);
                    Tiles.Add(new Point(x, y), new Tile(Tile.TileType.Ground, tx, ty));
                }
            }
            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes[2].ChildNodes) {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "Player") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value);
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value);
                    int id = Convert.ToInt32(xmlNode.Attributes["id"].Value);
                    player = new Player(this);
                    player.Pos = new Vector2(x, y);
                    Entities.Add(player);
                }
                else if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "ShellyEnemy") {
                    float x = float.Parse(xmlNode.Attributes["x"].Value);
                    float y = float.Parse(xmlNode.Attributes["y"].Value);
                    float patrolPt1 = float.Parse(xmlNode.Attributes["patrolPt1"].Value);
                    float patrolPt2 = float.Parse(xmlNode.Attributes["patrolPt2"].Value);
                    ShellyEnemy enemy = new ShellyEnemy(this, new Vector2(patrolPt1, y), new Vector2(patrolPt2, y));
                    enemy.Pos = new Vector2(x, y);
                    Entities.Add(enemy);
                }
                else if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "BasicSword") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value);
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value);
                    ItemEntity item = new ItemEntity(new BasicSword(), this, Images.BasicSword, 32, 32, 0);
                    item.Pos = new Vector2(x, y);
                    Entities.Add(item);
                }
                else if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "BeachWitchNpc") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value);
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value);
                    BeachWitchNpc witch = new BeachWitchNpc(this, new Sprite(Images.BeachWitchNpc, 64, 64, Consts.PlayerAnimFrameLength));
                    witch.Pos = new Vector2(x, y);
                    Entities.Add(witch);
                }
            }
            MediaPlayer.Play(Music.Back);
            return player;
        }

        public void Update(GameTime gameTime) {
            this.gameTime = gameTime;
            foreach (Entity entity in Entities) {
				entity.Update(gameTime);
			}	
			CheckCollision();
            foreach (Entity entity in entitiesToRemove) {
                Entities.Remove(entity);
            }
            entitiesToRemove.Clear();

            foreach (Particle particle in Particles) {
                particle.Update(gameTime);
                if (!particle.Alive) {
                    particlesToRemove.Add(particle);
                }
            }
            foreach (Particle particle in particlesToRemove) {
                Particles.Remove(particle);
            }
            particlesToRemove.Clear();
            if (DialogueBox != null) { DialogueBox.Update(gameTime); }
        }

        public void AddEntity(Entity e) {
            Entities.Add(e);
        }

        public void AddParticle(Particle p) {
            Particles.Add(p);
        }

        public void RemoveEntity(Entity e) {
            entitiesToRemove.Add(e);
        }

        private bool CollidesWith(Entity e1, Entity e2) {
            return (e1.Pos.X + e1.Dims.Width >= e2.Pos.X &&
                e1.Pos.Y + e1.Dims.Height >= e2.Pos.Y &&
                e2.Pos.X + e2.Dims.Width >= e1.Pos.X &&
                e2.Pos.Y + e2.Dims.Width >= e1.Pos.Y);
        }

        private void CheckCollision() {
            foreach (Entity e1 in Entities) {
                if (!e1.Alive) continue;
                foreach (Entity e2 in Entities) {
                    if (!e2.Alive) continue;
                    if (CollidesWith(e1, e2)) {
                        e1.CollisionWith(e2);
                        e2.CollisionWith(e1);
                    }
                }
            }
        }
    }
}