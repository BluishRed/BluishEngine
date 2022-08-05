using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine
{
    public class Map : World
    {
        public string Location { get; private set; }
        public Point Dimensions { get; private set; }
        public Rectangle Bounds { get; private set; }
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
            for (int layer = 0; layer < Layers.Count; layer++)
            {
                // TODO: Fix artifacts when zoomed
                foreach (TileLocation tileLocation in GetTilesInRegion(Camera.Viewport, layer))
                {
                    ComponentCollection tile = GetComponents(tileLocation.Tile);

                    spriteBatch.Draw(
                        texture: tile.GetComponent<Sprite>().Texture,
                        position: tileLocation.Position,
                        sourceRectangle: tile.GetComponent<Sprite>().Source,
                        color: Color.White
                    );
                }
            }
        }
        
        /// <summary>
        /// Gets all the tile types and locations from this <see cref="Map"/> at the given <paramref name="region"/> and <paramref name="layer"/>
        /// </summary>
        /// <returns>
        /// An iterable set of <see cref="TileLocation"/>, each containing the tile ID and its subsequent world location as a <see cref="Vector2"/>
        /// </returns>
        public HashSet<TileLocation> GetTilesInRegion(Rectangle region, int layer)
        {  
            region.Location = TileCoordinates(region.Location);
            region.Size = new Point((region.Size.X + TileDimensions.X - 1) / TileDimensions.X, (region.Size.Y + TileDimensions.Y +-1) / TileDimensions.Y);
            HashSet<TileLocation> tiles = new HashSet<TileLocation>();

            for (int y = region.Top; y <= region.Bottom; y++)
            {
                for (int x = region.Left; x <= region.Right; x++)
                {
                    if (Layers[layer][x, y] != 0)
                    {
                        tiles.Add(new TileLocation(WorldCoordinates(new Vector2(x, y)) + new Vector2(Camera.Focus.X % 1, Camera.Focus.Y % 1), Layers[layer][x, y]));
                    }
                }
            }

            return tiles;
        }

        /// <summary>
        /// Converts <paramref name="worldCoordinates"/> to its equivalent coordinate in terms of tiles
        /// </summary>
        /// <returns>
        /// A <see cref="Point"/> representing the tile coordinates of <paramref name="worldCoordinates"/>
        /// </returns>
        public Point TileCoordinates(Point worldCoordinates)
        {
            return new Point(worldCoordinates.X / TileDimensions.X, worldCoordinates.Y / TileDimensions.Y);
        }

        public Vector2 TileCoordinates(Vector2 worldCoordinates)
        {
            return new Vector2((int)(worldCoordinates.X / TileDimensions.X), (int)(worldCoordinates.Y / TileDimensions.Y));
        }

        public Point WorldCoordinates(Point tileCoordinates)
        {
            return new Point(tileCoordinates.X * TileDimensions.X, tileCoordinates.Y * TileDimensions.Y);
        }

        public Vector2 WorldCoordinates(Vector2 tileCoordinates)
        {
            return new Vector2((int)(tileCoordinates.X * TileDimensions.X), (int)(tileCoordinates.Y * TileDimensions.Y));
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

                // TODO: Make the dimensions of the map correlate to the dimensions of the midground (Or largest layer)?

                Dimensions = new Point(mapLayer.Width, mapLayer.Height);
                Bounds = new Rectangle(Point.Zero, Dimensions);

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

            // TODO: Check if they are sorted already anyway, so this is redundant
            Array.Sort(data.TileSets, (x, y) => x.FirstGID.CompareTo(y.FirstGID));

            foreach (TileSetReference tileSetReference in data.TileSets)
            {
                int id = tileSetReference.FirstGID;

                TileSetData tileSet = JsonSerializer.Deserialize<TileSetData>(File.ReadAllText(ContentProvider.RootDirectory + "/" + tileSetReference.Source), options);

                // TODO: Calculate the tile dimensions per layer

                TileDimensions = new Point(tileSet.TileWidth, tileSet.TileHeight);

                for (int y = 0; y < tileSet.ImageHeight; y += tileSet.TileHeight)
                {
                    for  (int x = 0; x < tileSet.ImageWidth; x += tileSet.TileWidth)
                    {
                        AddEntity(
                            new Sprite(Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(tileSetReference.Source), tileSet.Image), null), new Rectangle(x, y, tileSet.TileWidth, tileSet.TileHeight)), 
                            new Dimensions(tileSet.TileWidth, tileSet.TileHeight)
                        );

                        id++;
                    }
                }
                
                if (tileSet.Tiles is not null)
                {
                    foreach (Tiles tile in tileSet.Tiles)
                    {
                        AddComponent(tile.ID + tileSetReference.FirstGID, new Collidable(new Rectangle(tile.ObjectGroup.Objects[0].X, tile.ObjectGroup.Objects[0].Y, tile.ObjectGroup.Objects[0].Width, tile.ObjectGroup.Objects[0].Height)));
                    }
                }
            }

            Dimensions = new Point(Dimensions.X * TileDimensions.X, Dimensions.Y * TileDimensions.Y);

            AddSystems();

            Camera.Bounds = new Rectangle(Point.Zero, Dimensions);

            base.LoadContent(content);
        }

        private void AddSystems()
        {
            AddSystem(new Systems.SpriteLoader(this));
        }

        /// <summary>
        /// Represents a particular tile on the map as its tile ID and its world location
        /// </summary>
        public class TileLocation
        {
            public Vector2 Position { get; private set; }
            public Entity Tile { get; private set; }

            public TileLocation(Vector2 position, int tile)
            {
                Position = position;
                Tile = tile;
            }
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
            public Tiles[] Tiles { get; set; }
        }

        private class Tiles
        { 
            public int ID { get; set; }
            public TileObjectGroup ObjectGroup { get; set; }
        }

        private class TileObjectGroup
        { 
            public TileObject[] Objects { get; set; }
        }

        private class TileObject
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
        #endregion
    }
}