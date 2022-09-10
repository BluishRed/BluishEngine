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
    /// <summary>
    /// A <see cref="World"/> containing data on tiles and the structure of them in the form of layers
    /// </summary>
    public class Map : World
    {
        /// <summary>
        /// Content location
        /// </summary>
        public string Location { get; private set; }
        // TODO: Add summary
        public Point Dimensions { get; private set; }
        /// <summary>
        /// A <see cref="Rectangle"/> with location <see cref="Point.Zero"/> and a size of <see cref="Dimensions"/>
        /// </summary>
        public Rectangle Bounds { get; private set; }
        protected List<Entity[,]> Layers { get; private set; }
        protected List<float> Depths { get; private set; }
        protected List<Rectangle> Rooms { get; private set; }
        protected Camera Camera { get; private set; }
        protected Point TileDimensions { get; private set; }
        
        public Map(string location, Camera camera)
        {
            Location = location;
            Layers = new List<Entity[,]>();
            Rooms = new List<Rectangle>();
            Depths = new List<float>();
            Camera = camera;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        { 
            for (int layer = 0; layer < Layers.Count; layer++)
            {
                foreach (TileLocation tileLocation in GetTilesInRegion(Camera.Viewport, layer))
                {
                    ComponentCollection tile = GetComponents(tileLocation.Tile);

                    spriteBatch.Draw(
                        texture: tile.GetComponent<Sprite>().Texture,
                        position: tileLocation.Position,
                        sourceRectangle: tile.GetComponent<Sprite>().Source,
                        color: Color.White,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1,
                        effects: SpriteEffects.None,
                        layerDepth: Depths[layer]
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
            region.Size = new Point((region.Size.X + TileDimensions.X - 1) / TileDimensions.X, (region.Size.Y + TileDimensions.Y - 1) / TileDimensions.Y);
            HashSet<TileLocation> tiles = new HashSet<TileLocation>();

            for (int y = region.Top; y <= region.Bottom; y++)
            {
                for (int x = region.Left; x <= region.Right; x++)
                {
                    if (Layers[layer][x, y] != 0)
                    {
                        tiles.Add(new TileLocation(WorldCoordinates(new Vector2(x, y)), Layers[layer][x, y]));
                    }
                }
            }

            return tiles;
        }

        public Rectangle GetRoomContainingVector(Vector2 vector2)
        {
            // TODO: Optimise this

            foreach (Rectangle room in Rooms)
            {
                if (room.Contains(vector2))
                {
                    return room;
                }
            }
            throw new Exception($"There is no room that contains {vector2}");
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

            Depths.Add(0f);

            foreach (MapLayerData mapLayer in data.Layers)
            {
                if (mapLayer.Type == "tilelayer")
                {
                    int tile = 0;
                    Layers.Add(new Entity[mapLayer.Width, mapLayer.Height]);

                    Depths.Add(Depths[^1] + 1f / data.Layers.Length);

                    // TODO: Make the dimensions of the map correlate to the dimensions of the midground (Or largest layer)?
                     
                    Dimensions = new Point(mapLayer.Width, mapLayer.Height);
                    Bounds = new Rectangle(Point.Zero, Dimensions);

                    for (int y = 0; y < mapLayer.Height; y++)
                    {
                        for (int x = 0; x < mapLayer.Width; x++)
                        {
                            Layers[^1][x, y] = mapLayer.Data[tile];
                            tile++;
                        }
                    }
                } 
                else if (mapLayer.Type == "objectgroup")
                {
                    if (mapLayer.Name == "Rooms")
                    {
                        foreach (Object room in mapLayer.Objects)
                        {
                            Rooms.Add(new Rectangle(room.X, room.Y, room.Width, room.Height));
                        }
                    }
                }
            }

            Depths.RemoveAt(0);
            
            // Loading Tilesets

            AddEntity();

            foreach (TileSetReference tileSetReference in data.TileSets)
            {
                int id = tileSetReference.FirstGID;

                TileSetData tileSet = JsonSerializer.Deserialize<TileSetData>(File.ReadAllText(ContentProvider.RootDirectory + "/" + Path.Combine(Path.GetDirectoryName(Location), tileSetReference.Source)), options);

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
                    foreach (Tile tile in tileSet.Tiles)
                    {
                        foreach (Object tileObject in tile.ObjectGroup.Objects)
                        {
                            if (tileObject.Type == "Collidable")
                            {
                                bool jumpThrough = false;

                                if (tileObject.Properties is not null)
                                {
                                    List<ObjectProperty> properties = tileObject.Properties.ToList();

                                    jumpThrough = properties.FindIndex(o => o.Name == "JumpThrough" && o.Value.GetBoolean()) != -1;
                                }

                                AddComponent(tile.ID + tileSetReference.FirstGID, new Collidable(new Rectangle(tile.ObjectGroup.Objects[0].X, tile.ObjectGroup.Objects[0].Y, tile.ObjectGroup.Objects[0].Width, tile.ObjectGroup.Objects[0].Height), jumpThrough ? new HashSet<Direction>() { Direction.Up, Direction.Left, Direction.Right } : null));
                            }
                        }
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
            return new Vector2(tileCoordinates.X * TileDimensions.X, tileCoordinates.Y * TileDimensions.Y);
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
            public string Name { get; set; }
            public int[] Data { get; set; }
            public Object[] Objects { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool Visible { get; set; }
            public string Type { get; set; }
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
            public Tile[] Tiles { get; set; }
        }

        private class Tile
        { 
            public int ID { get; set; }
            public TileObjectGroup ObjectGroup { get; set; }
        }

        private class TileObjectGroup
        { 
            public Object[] Objects { get; set; }
        }

        private class Object
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public string Type { get; set; }
            public ObjectProperty[] Properties { get; set; }
        }

        private class ObjectProperty
        { 
            public string Name { get; set; }
            public string Type { get; set; }
            public JsonElement Value { get; set; }
        }
        #endregion
    }
}