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
        public Point Dimensions { get; private set; }
        protected List<Entity[,]> Layers { get; private set; }
        protected Camera Camera { get; private set; }
        protected Point TileDimensions { get; private set; }
        
        public Map(string location, Camera camera)
        {
            Location = location;
            Layers = new List<Entity[,]>();
            Camera = camera;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle viewport = Camera.GetViewport();

            foreach (Entity[,] layer in Layers)
            {
                for (int y = viewport.Y / TileDimensions.Y; y < Math.Ceiling((viewport.Y + viewport.Height) / (double)TileDimensions.Y); y++)
                {
                    for (int x = viewport.X / TileDimensions.X; x < Math.Ceiling((viewport.X + viewport.Width) / (double)TileDimensions.X); x++)
                    {
                        if (x >= 0 && y >= 0 && layer[x, y] != 0)
                        {
                            ComponentCollection tile = GetComponents(layer[x, y]);
                            
                            spriteBatch.Draw(
                                texture: tile.GetComponent<Components.Sprite>().Texture,
                                position: new Vector2(x * TileDimensions.X, y * TileDimensions.Y),
                                sourceRectangle: tile.GetComponent<Components.Sprite>().Source, Color.White
                            );
                        }
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
            foreach (MapLayerData mapLayer in data.Layers)
            {
                int tile = 0;
                Layers.Add(new Entity[mapLayer.Width, mapLayer.Height]);

                // TODO: Make the dimensions of the map correlate to the dimensions of the midground

                Dimensions = new Point(mapLayer.Width, mapLayer.Height);

                for (int y = 0; y < Layers[layer].GetLength(0); y++)
                {
                    for (int x = 0; x < Layers[layer].GetLength(1); x++)
                    {
                        Layers[layer][x, y] = data.Layers[layer].Data[tile];
                        tile++;
                    }
                }
                layer++;
            }

            // Loading Tilesets

            AddEntity();

            Array.Sort(data.TileSets, (x, y) => x.FirstGID.CompareTo(y.FirstGID));

            foreach (TileSetReference tileSetReference in data.TileSets)
            {
                int id = tileSetReference.FirstGID;

                TileSetData tileSet = JsonSerializer.Deserialize<TileSetData>(File.ReadAllText(ContentProvider.RootDirectory + "/" + tileSetReference.Source), options);

                TileDimensions = new Point(tileSet.TileWidth, tileSet.TileHeight);

                for (int y = 0; y < tileSet.ImageHeight; y += tileSet.TileHeight)
                {
                    for (int x = 0; x < tileSet.ImageWidth; x += tileSet.TileWidth)
                    {
                        AddEntity(new Components.Sprite(Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(tileSetReference.Source), tileSet.Image), null), new Rectangle(x, y, tileSet.TileWidth, tileSet.TileHeight)), new Components.Dimensions(tileSet.TileWidth, tileSet.TileHeight));
                        id++;
                    }
                }
            }

            AddSystem(new Systems.SpriteLoader(this));

            base.LoadContent(content);
        }

        #region Map data classes
        private class MapData
        {
            public MapLayerData[] Layers { get; set; }
            public TileSetReference[] TileSets { get; set; }
        }

        private class MapLayerData
        {
            public int[] Data { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool Visible { get; set; }
        }

        private class TileSetReference
        {
            public int FirstGID { get; set; }
            public string Source { get; set; }
        }

        private class TileSetData
        {
            public string Image { get; set; }   
            public int TileWidth { get; set; }
            public int TileHeight { get; set; }
            public int ImageWidth { get; set; }
            public int ImageHeight { get; set; }
        }
        #endregion
    }
}