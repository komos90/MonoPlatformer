using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    public class World {
        public Player player;
        public TileSpriteSheet TileSpriteSheet { get; private set; }
        public Dictionary<Point, Tile> BackgroundTiles { get; private set; }
        public Dictionary<Point, Tile> Tiles { get; private set; }
        public Dictionary<Point, Tile> MidTiles { get; private set; }
        public Dictionary<Point, Tile> ForegroundTiles { get; private set; }
        public List<Entity> Entities { get; private set; }
        List<Entity> entitiesToRemove;
        public List<Particle> Particles { get; private set; }
        List<Particle> particlesToRemove;
        public GameTime gameTime;
        public Random rand;
        public Dictionary<string, string> StoryVars;
        public Dialogue DialogueBox;
        public List<Background> backgrounds;
        public List<Background> foregrounds;

        public World(TileSpriteSheet tileSpriteSheet) {
            TileSpriteSheet = tileSpriteSheet;
            BackgroundTiles = new Dictionary<Point, Tile>();
            Tiles = new Dictionary<Point, Tile>();
            MidTiles = new Dictionary<Point, Tile>();
            ForegroundTiles = new Dictionary<Point, Tile>();
            Entities = new List<Entity>();
            entitiesToRemove = new List<Entity>();
            Particles = new List<Particle>();
            particlesToRemove = new List<Particle>();
            rand = new Random(); //Should seed
            StoryVars = new Dictionary<string, string>();
            backgrounds = new List<Background>();
            foregrounds = new List<Background>();
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
                    Console.WriteLine("No Such Entity");
                    break;
            }
            Entities.Add(entity);
        }

        public Player LoadLevel(string path) {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(path + ".oel");
            var levelExtraXml = new XmlDocument();
            levelExtraXml.Load(path + ".extra");
            XmlNode offsetNode = levelExtraXml.DocumentElement.GetElementsByTagName("offset")[0];
            int levelxoff = Convert.ToInt32(offsetNode.Attributes["x"].Value);
            int levelyoff = Convert.ToInt32(offsetNode.Attributes["y"].Value);

            foreach (XmlNode xmlNode in levelExtraXml.DocumentElement.GetElementsByTagName("backgrounds")[0].ChildNodes) {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "background") {
                    string image_key = xmlNode.Attributes["image"].Value;
                    float xrate = Convert.ToSingle(xmlNode.Attributes["xrate"].Value);
                    float yrate = Convert.ToSingle(xmlNode.Attributes["yrate"].Value);
                    float xoff = Convert.ToSingle(xmlNode.Attributes["xoff"].Value);
                    float yoff = Convert.ToSingle(xmlNode.Attributes["yoff"].Value);
                    bool xloops = Convert.ToBoolean(xmlNode.Attributes["xloops"].Value);
                    bool yloops = Convert.ToBoolean(xmlNode.Attributes["yloops"].Value);
                    backgrounds.Add(new Background(Images.GetImage(image_key), xrate, yrate, xoff, yoff, xloops, yloops));
                }
            }
            {
                XmlNode tileLayerNode = xmlDocument.DocumentElement.GetElementsByTagName("back_tile_layer")[0];
                string tileSheetKey = tileLayerNode.Attributes["tileset"].Value;
                foreach (XmlNode xmlNode in tileLayerNode.ChildNodes) {
                    if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "tile") {
                        int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff;
                        int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff;
                        int tx = Convert.ToInt32(xmlNode.Attributes["tx"].Value);
                        int ty = Convert.ToInt32(xmlNode.Attributes["ty"].Value);
                        BackgroundTiles.Add(new Point(x, y), new Tile(Tile.TileType.Air, tx, ty, tileSheetKey));
                    }
                }
            }
            {
                XmlNode tileLayerNode = xmlDocument.DocumentElement.GetElementsByTagName("func_tile_layer")[0];
                string tileSheetKey = tileLayerNode.Attributes["tileset"].Value;
                foreach (XmlNode xmlNode in tileLayerNode.ChildNodes) {
                    if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "tile") {
                        int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff;
                        int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff;
                        int id = Convert.ToInt32(xmlNode.Attributes["id"].Value);
                        Tiles.Add(new Point(x, y), new Tile((Tile.TileType)id, id, 0, tileSheetKey));
                    }
                }
            }
            {
                XmlNode tileLayerNode = xmlDocument.DocumentElement.GetElementsByTagName("tile_layer")[0];
                string tileSheetKey = tileLayerNode.Attributes["tileset"].Value;
                foreach (XmlNode xmlNode in tileLayerNode.ChildNodes) {
                    if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "tile") {
                        int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff;
                        int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff;
                        int tx = Convert.ToInt32(xmlNode.Attributes["tx"].Value);
                        int ty = Convert.ToInt32(xmlNode.Attributes["ty"].Value);
                        MidTiles.Add(new Point(x, y), new Tile(Tile.TileType.Air, tx, ty, tileSheetKey));
                    }
                }
            }
            {
                XmlNode tileLayerNode = xmlDocument.DocumentElement.GetElementsByTagName("fore_tile_layer")[0];
                string tileSheetKey = tileLayerNode.Attributes["tileset"].Value;
                foreach (XmlNode xmlNode in tileLayerNode.ChildNodes) {
                    if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "tile") {
                        int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff;
                        int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff;
                        int tx = Convert.ToInt32(xmlNode.Attributes["tx"].Value);
                        int ty = Convert.ToInt32(xmlNode.Attributes["ty"].Value);
                        ForegroundTiles.Add(new Point(x, y), new Tile(Tile.TileType.Air, tx, ty, tileSheetKey));
                    }
                }
            }
            
            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.GetElementsByTagName("entities")[0].ChildNodes) {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "Player") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff * Tile.Width;
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff * Tile.Width;
                    int id = Convert.ToInt32(xmlNode.Attributes["id"].Value);
                    player = new Player(this);
                    player.Pos = new Vector2(x, y);
                    Entities.Add(player);
                }
                else if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "ShellyEnemy") {
                    float x = float.Parse(xmlNode.Attributes["x"].Value) + levelxoff * Tile.Width;
                    float y = float.Parse(xmlNode.Attributes["y"].Value) + levelyoff * Tile.Width;
                    float patrolPt1 = float.Parse(xmlNode.Attributes["patrolPt1"].Value) + levelxoff * Tile.Width; 
                    float patrolPt2 = float.Parse(xmlNode.Attributes["patrolPt2"].Value) + levelyoff * Tile.Width;
                    ShellyEnemy enemy = new ShellyEnemy(this, new Vector2(patrolPt1, y), new Vector2(patrolPt2, y));
                    enemy.Pos = new Vector2(x, y);
                    Entities.Add(enemy);
                }
                else if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "BasicSword") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff * Tile.Width;
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff * Tile.Width;
                    ItemEntity item = new ItemEntity(new BasicSword(), this, Images.GetImage("basic_sword"), 32, 32, 0);
                    item.Pos = new Vector2(x, y);
                    Entities.Add(item);
                }
                else if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "BeachWitchNpc") {
                    int x = Convert.ToInt32(xmlNode.Attributes["x"].Value) + levelxoff * Tile.Width;
                    int y = Convert.ToInt32(xmlNode.Attributes["y"].Value) + levelyoff * Tile.Width;
                    BeachWitchNpc witch = new BeachWitchNpc(this, new Sprite(Images.GetImage("beach_witch_npc"), 64, 64, Consts.PlayerAnimFrameLength));
                    witch.Pos = new Vector2(x, y);
                    Entities.Add(witch);
                }
            }
            foreach (XmlNode xmlNode in levelExtraXml.DocumentElement.GetElementsByTagName("foregrounds")[0].ChildNodes) {
                if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name == "background") {
                    string image_key = xmlNode.Attributes["image"].Value;
                    float xrate = Convert.ToSingle(xmlNode.Attributes["xrate"].Value);
                    float yrate = Convert.ToSingle(xmlNode.Attributes["yrate"].Value);
                    float xoff = Convert.ToSingle(xmlNode.Attributes["xoff"].Value);
                    float yoff = Convert.ToSingle(xmlNode.Attributes["yoff"].Value);
                    bool xloops = Convert.ToBoolean(xmlNode.Attributes["xloops"].Value);
                    bool yloops = Convert.ToBoolean(xmlNode.Attributes["yloops"].Value);
                    foregrounds.Add(new Background(Images.GetImage(image_key), xrate, yrate, xoff, yoff, xloops, yloops));
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

        public void Draw(SpriteBatch spriteBatch, Camera camera, Renderer renderer) {
            foreach(var back in backgrounds) {
                back.Draw(spriteBatch, camera);
            }
            {
                var i = BackgroundTiles.GetEnumerator();
                while (i.MoveNext()) {
                    KeyValuePair<Point, Tile> curTile = i.Current;
                    curTile.Value.Draw(curTile.Key, camera, spriteBatch, renderer);
                }
            }
            {
                var i = MidTiles.GetEnumerator();
                while (i.MoveNext()) {
                    KeyValuePair<Point, Tile> curTile = i.Current;
                    curTile.Value.Draw(curTile.Key, camera, spriteBatch, renderer);
                }
            }
            foreach (Entity entity in Entities) {
                entity.Draw(camera, spriteBatch, renderer);
            }
            foreach (Particle particle in Particles) {
                particle.Draw(camera, spriteBatch);
            }
            {
                var i = ForegroundTiles.GetEnumerator();
                while (i.MoveNext()) {
                    KeyValuePair<Point, Tile> curTile = i.Current;
                    curTile.Value.Draw(curTile.Key, camera, spriteBatch, renderer);
                }
            }
            foreach (var fore in foregrounds) {
                fore.Draw(spriteBatch, camera);
            }
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
