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
        protected List<Layer> Layers { get; private set; }
        protected List<Room> Rooms { get; private set; }
        protected Camera Camera { get; private set; }
        protected Point TileDimensions { get; private set; }

        public Map(string location, Camera camera)
        {
            Location = location;
            Layers = new List<Layer>();
            Rooms = new List<Room>();
            Camera = camera;
        }

        // TODO: Flashing tile in bottom left when zooming in in first room
        // TODO: Implement Parallax
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int layer = 0; layer < Layers.Count; layer++)
            {
                if (!Layers[layer].Visible) continue;

                foreach (TileLocation tileLocation in GetTilesInRegion(Camera.Viewport, layer))
                {
                    ComponentCollection tile = GetComponents(tileLocation.Tile);

                    spriteBatch.Draw(
                        texture: tile.GetComponent<Sprite>().Texture,
                        position: tileLocation.Position,
                        sourceRectangle: tile.GetComponent<Sprite>().Source,
                        color: new Color(1, 1, 1, Layers[layer].Depth),
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: 1,
                        effects: SpriteEffects.None,
                        Layers[layer].Depth
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
        public HashSet<TileLocation> GetTilesInRegion(RectangleF region, float depth)
        {
            return GetTilesInRegion(region, GetLayerIndex(depth));
        }

        public HashSet<TileLocation> GetTilesInRegion(Rectangle region, float depth)
        {
            return GetTilesInRegion(new RectangleF(region.Location.ToVector2(), region.Size.ToVector2()), depth);
        }

        // TODO: Check if RectangleF is actually necessary
        private HashSet<TileLocation> GetTilesInRegion(RectangleF region, int layer)
        {
            region.Location = TileCoordinates(region.Location);
            region.Size = Vector2.Ceiling(new Vector2(region.Size.X / TileDimensions.X, region.Size.Y / TileDimensions.Y));
            HashSet<TileLocation> tiles = new HashSet<TileLocation>();

            for (float y = region.Top; y <= region.Bottom; y++)
            {
                for (float x = region.Left; x <= region.Right; x++)
                {
                    if (Layers[layer].Tiles[(int)x, (int)y] != 0)
                    {
                        tiles.Add(new TileLocation(WorldCoordinates(new Vector2(x, y)), Layers[layer].Tiles[(int)x, (int)y]));
                    }
                }
            }

            return tiles;
        }

        public Rectangle GetRoomContainingVector(Vector2 vector2)
        {
            // TODO: Optimise this

            foreach (Room room in Rooms)
            {
                if (room.Bounds.Contains(vector2))
                {
                    return room.Bounds;
                }
            }
            throw new Exception($"There is no room that contains {vector2}");
        }
        
        private int GetLayerIndex(float depth)
        {
            int layer = 0;
            float delta = float.MaxValue;

            for (int i = 0; i < Layers.Count; i++)
            {
                if (depth > Layers[i].Depth && depth - Layers[i].Depth < delta)
                {
                    delta = depth - Layers[i].Depth;
                    layer = i;
                    if (delta == 0)
                    {
                        return layer;
                    }
                }
            }

            return layer;
        }

        // TODO: Make layer depth an explicit property in Tiled
        // TODO: Make tiles larger than 1x1 be parsed
        public override void LoadContent(ContentManager content)
        {
            // Reading Data

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            MapData data = JsonSerializer.Deserialize<MapData>(File.ReadAllText(ContentProvider.RootDirectory + "/" + Location), options);

            // Initialising Map Layers

            foreach (MapLayerData mapLayer in data.Layers)
            {
                if (mapLayer.Type == "tilelayer")
                {
                    int tile = 0;
                    Entity[,] tiles = new Entity[mapLayer.Width, mapLayer.Height];

                    for (int y = 0; y < mapLayer.Height; y++)
                    {
                        for (int x = 0; x < mapLayer.Width; x++)
                        {
                            tiles[x, y] = mapLayer.Data[tile];
                            tile++;
                        }
                    }

                    Layers.Add(
                        new Layer(
                            tiles,
                            (Layers.Count > 0 ? Layers[^1].Depth : 0) + 1f / data.Layers.Length, 
                            mapLayer.Visible,
                            new Vector2(mapLayer.ParallaxX, mapLayer.ParallaxY)
                        )
                    );
                }
                else if (mapLayer.Type == "objectgroup")
                {
                    if (mapLayer.Name == "Rooms")
                    {
                        foreach (Object room in mapLayer.Objects)
                        {
                            // TODO: Ambient colour is flipped
                            Rooms.Add(new Room(new Rectangle(room.X, room.Y, room.Width, room.Height)));
                        }
                    }
                }
            }

            // Loading Tilesets

            AddEntity();

            const int padding = 1;

            foreach (TileSetReference tileSetReference in data.TileSets)
            {
                int id = tileSetReference.FirstGID;

                TileSetData tileSet = JsonSerializer.Deserialize<TileSetData>(File.ReadAllText(ContentProvider.RootDirectory + "/" + Path.Combine(Path.GetDirectoryName(Location), tileSetReference.Source)), options);

                TileDimensions = new Point(tileSet.TileWidth, tileSet.TileHeight);

                for (int y = padding; y < tileSet.ImageHeight; y += tileSet.TileHeight + 2 * padding)
                {
                    for  (int x = padding; x < tileSet.ImageWidth; x += tileSet.TileWidth + 2 * padding)
                    {
                        string spriteLocation = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(tileSetReference.Source), tileSet.Image), null);
                        AddEntity(
                            new Sprite("Maps/" + spriteLocation, new Rectangle(x, y, tileSet.TileWidth, tileSet.TileHeight)), 
                            new Dimensions(tileSet.TileWidth, tileSet.TileHeight)
                        );

                        id++;
                    }
                }
                
                // Adding additional components

                if (tileSet.Tiles is not null)
                {
                    foreach (Tile tile in tileSet.Tiles)
                    {
                        // Animation

                        Frame[] animation = tile.Animation;

                        if (animation is not null)
                        {
                            List<(Rectangle, float)> frames = new List<(Rectangle, float)>();

                            foreach (Frame frame in animation)
                            {
                                frames.Add((new Rectangle(padding + (tileSet.TileWidth + 2 * padding) * (frame.TileID % (tileSet.ImageWidth / (tileSet.TileWidth + 2 * padding))), padding + (tileSet.TileHeight + 2 * padding) * (frame.TileID / (tileSet.ImageHeight / (tileSet.TileHeight + 2 * padding))), tileSet.TileWidth, tileSet.TileHeight), frame.Duration / 1000));
                            }

                            AddComponent(tile.ID + tileSetReference.FirstGID, new PassivelyAnimated(frames.ToArray()));
                        }

                        // Objects

                        if (tile.ObjectGroup is not null)
                        {

                            // Collision

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
            }

            AddSystems();

            base.LoadContent(content);
        }

        private void AddSystems()
        {
            AddSystem(new Systems.SpriteLoader(this));
            AddSystem(new Systems.PassiveAnimation(this));
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

        protected class Layer
        {
            public Entity[,] Tiles { get; private set; }
            public float Depth { get; private set; }
            public Vector2 Parallax { get; private set; }
            public bool Visible { get; private set; }
            public Point Dimensions => new Point(Tiles.GetLength(0), Tiles.GetLength(1));

            public Layer(Entity[,] tiles, float depth, bool visible, Vector2 parallax)
            {
                Tiles = tiles;
                Depth = depth;
                Visible = visible;
                Parallax = parallax;
            }
        }
        protected class Room
        {
            public Rectangle Bounds { get; private set; }

            public Room(Rectangle bounds)
            {
                Bounds = bounds;
            }
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
            public float ParallaxX { get; set; } = 1;
            public float ParallaxY { get; set; } = 1;
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
            public Frame[] Animation { get; set; }
            public TileObjectGroup ObjectGroup { get; set; }
        }

        private class Frame
        {
            public float Duration { get; set; }
            public int TileID { get; set; }
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