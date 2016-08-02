using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;

namespace Platformer {
    public class World {
        private Player player;
        public TileSpriteSheet TileSpriteSheet { get; private set; }
        public Dictionary<Point, Tile> Tiles { get; private set; }
        public List<Entity> Entities { get; private set; }
        List<Entity> entitiesToRemove;
        public GameTime gameTime;

        public World(TileSpriteSheet tileSpriteSheet) {
            TileSpriteSheet = tileSpriteSheet;
            Tiles = new Dictionary<Point, Tile>();
            Entities = new List<Entity>();
            entitiesToRemove = new List<Entity>();
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
            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes[0].ChildNodes) {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "tile") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value);
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value);
                    int tx = Convert.ToInt32(xmlNode.Attributes["tx"].Value);
                    int ty = Convert.ToInt32(xmlNode.Attributes["ty"].Value);
                    Tiles.Add(new Point(x, y), new Tile(Tile.TileType.Ground, tx, ty));
                }
            }
            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.ChildNodes[1].ChildNodes) {
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
            }
            return player;
        }

        public void Update(GameTime gameTime) {
            this.gameTime = gameTime;
            foreach (Entity entity in Entities) {
				entity.Update(gameTime);
				//Physics.Gravity(entity);
			}	

			CheckCollision();

            foreach (Entity entity in entitiesToRemove) {
                Entities.Remove(entity);
                //Physics.Gravity(entity);
            }
            entitiesToRemove.Clear();
        }

        public void AddEntity(Entity e) {
            Entities.Add(e);
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
