using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;


namespace BluishEngine
{
    public abstract class BluishState : State
    {
        public void AddMap(string location)
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            Map map = JsonSerializer.Deserialize<Map>(File.ReadAllText(location), options);

            Dictionary<int, (string, Rectangle)> tiles = new Dictionary<int, (string, Rectangle)>();

            foreach (TileSet tileSet in map.TileSets)
            {
                int id = tileSet.FirstGID;

                for (int y = 0; y < tileSet.ImageHeight; y += tileSet.TileHeight)
                {
                    for (int x = 0; x < tileSet.ImageWidth; x += tileSet.TileWidth)
                    {
                        tiles.Add(id, (Path.Combine(location, tileSet.Image), new Rectangle(x, y, tileSet.TileWidth, tileSet.TileHeight)));
                        id++;
                    }
                }
            }


        }

        #region Map data classes
        private class Map
        {
            public MapLayer[] Layers { get; set; }
            public TileSet[] TileSets { get; set; }
        }

        private class MapLayer
        {
            public int[] Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool Visible { get; set; }
        }

        private class TileSet
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