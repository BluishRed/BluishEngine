using BluishFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json;

namespace BluishEngine
{
    public class Map : World
    {
        public string Location { get; private set; }
        protected List<Entity[,]> Layers { get; private set; }

        public Map(string location)
        {
            Location = location;
            Layers = new List<Entity[,]>();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity[,] layer in Layers)
            {
                for (int y = 0; y < 180; y++)
                {
                    for (int x = 0; x < 320; x++)
                    {
                        if (layer[x, y] != 0)
                            spriteBatch.Draw(GetComponents(layer[x, y]).GetComponent<Components.Sprite>().Texture, new Vector2(x * 20, y * 20), GetComponents(layer[x, y]).GetComponent<Components.Sprite>().Source, Color.White);
                    }
                }
            }
        }

        public override void LoadContent(ContentManager content)
        {
            // Reading Data

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            MapData data = JsonSerializer.Deserialize<MapData>(File.ReadAllText(ContentProvider.RootDirectory + "/" + Location), options);

            // Initialising Map Layers

            int layer = 0;
            foreach (MapLayerData mapLayerData in data.Layers)
            {
                int index = 0;
                Layers.Add(new Entity[mapLayerData.Width, mapLayerData.Height]);

                for (int y = 0; y < Layers[layer].GetLength(0); y++)
                {
                    for (int x = 0; x < Layers[layer].GetLength(1); x++)
                    {
                        Layers[layer][x, y] = data.Layers[layer].Data[index];
                        index++;
                    }
                }
                layer++;
            }

            // Loading Tilesets

            AddEntity();

            Array.Sort(data.TileSets, (x, y) => x.FirstGID.CompareTo(y.FirstGID));

            foreach (TileSetData tileSet in data.TileSets)
            {
                int id = tileSet.FirstGID;

                for (int y = 0; y < tileSet.ImageHeight; y += tileSet.TileHeight)
                {
                    for (int x = 0; x < tileSet.ImageWidth; x += tileSet.TileWidth)
                    {
                        AddEntity(new Components.Sprite(Path.ChangeExtension(tileSet.Image, null), new Rectangle(x, y, tileSet.TileWidth, tileSet.TileHeight)));
                        id++;
                    }
                }
            }

            // TODO: Automate the adding of loading systems
            // Also, test of using hp laptop remote repo

            AddSystem<Systems.SpriteLoader>();

            base.LoadContent(content);
        }

        #region Map data classes
        private class MapData
        {
            public MapLayerData[] Layers { get; set; }
            public TileSetData[] TileSets { get; set; }
        }

        private class MapLayerData
        {
            public int[] Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool Visible { get; set; }
        }

        private class TileSetData
        {
            public string Image { get; set; }
            public int FirstGID { get; set; }
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public int ImageWidth { get; set; }
            public int ImageHeight { get; set; }
        }
        #endregion
    }
}