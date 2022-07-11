using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public class Map : World
    {
        public string Location { get; private set; }
        protected List<int[,]> Layers { get; private set; }

        public Map(string location)
        {
            Location = location;
            Layers = new List<int[,]>();
        }

        protected override void LoadContent(ContentManager content)
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
                Layers.Add(new int[mapLayerData.Width, mapLayerData.Height]);

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

            //Tiles.Add(0, Tile.Empty);

            //foreach (TileSetData tileSetData in data.TileSets)
            //{
            //    int id = tileSetData.FirstGID;
            //    Spritesheet tileset = ContentLoader.LoadSpritesheet(content, Path.Combine(ContentLocation, tileSetData.Image), TileDimensions);

            //    for (int y = 0; y < tileSetData.ImageHeight; y += TileDimensions.Height)
            //    {
            //        for (int x = 0; x < tileSetData.ImageWidth; x += TileDimensions.Width)
            //        {
            //            Tiles.Add(id, new Tile(tileset, new Rectangle(x, y, TileDimensions.Width, TileDimensions.Height)));
            //            id++;
            //        }
            //    }
            //}

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

        private class TileData
        {
            public string Location { get; set; }
            public Rectangle Source { get; set; }

            public TileData(string location, Rectangle source)
            {
                Location = location;
                Source = source;
            }
        }
        #endregion
    }
}