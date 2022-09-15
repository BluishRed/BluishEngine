using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class MapCollision : UpdateSystem
    {
        protected Map Map { get; private set; }

        public MapCollision(World world, Map map) : base(world, typeof(Collidable), typeof(KinematicBody), typeof(Transform))
        {
            Map = map;
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            // TODO: Make entity collide with bounds of rooms?

            Vector2 position = components.GetComponent<Transform>().Position + components.GetComponent<Collidable>().BoundingBox.Location.ToVector2();
            bool onGround = components.GetComponent<Collidable>().OnGround;

            ResolveMapCollisions(
                components.GetComponent<Collidable>().BoundingBox.Width,
                components.GetComponent<Collidable>().BoundingBox.Height,
                components.GetComponent<Transform>().Depth,
                ref position,
                ref components.GetComponent<KinematicBody>().Velocity,
                ref components.GetComponent<KinematicBody>().Acceleration,
                ref onGround
            );

            components.GetComponent<Collidable>().OnGround = onGround;
            components.GetComponent<Transform>().Position = position - components.GetComponent<Collidable>().BoundingBox.Location.ToVector2();
        }

        protected void ResolveMapCollisions(int width, int height, float depth, ref Vector2 position, ref Vector2 velocity, ref Vector2 acceleration, ref bool onGround)
        {
            if (velocity.X != 0)
            {
                Rectangle check;

                if (velocity.X < 0)
                {
                    check = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), (int)Math.Round(-velocity.X), height);

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth, Direction.Left);

                    if (collidableTiles.Count > 0)
                    {
                        velocity.X = 0;
                        acceleration.X = 0;
                        position.X += Math.Min(0, MaxX(collidableTiles) - position.X);
                    }
                }
                else
                {
                    check = new Rectangle((int)Math.Ceiling(position.X + width), (int)(position.Y + velocity.Y), (int)Math.Round(velocity.X), height);

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth, Direction.Right);

                    if (collidableTiles.Count > 0)
                    {
                        velocity.X = 0;
                        acceleration.X = 0;
                        position.X += Math.Max(0, MinX(collidableTiles) - position.X - width);
                    }
                }
            }

            onGround = false;

            if (velocity.Y != 0)
            {
                Rectangle check;

                if (velocity.Y < 0)
                {
                    check = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), width, (int)Math.Ceiling(-velocity.Y));

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth, Direction.Up);

                    if (collidableTiles.Count > 0)
                    {
                        velocity.Y = 0;
                        acceleration.Y = 0;
                        position.Y += Math.Min(0, MaxY(collidableTiles) - position.Y);
                    }
                }
                else
                {
                    check = new Rectangle((int)(position.X + velocity.X), (int)(position.Y + height + velocity.Y), width, (int)Math.Ceiling(velocity.Y));

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth, Direction.Down);

                    if (collidableTiles.Count > 0)
                    {
                        velocity.Y = 0;
                        acceleration.Y = 0;
                        position.Y += Math.Max(0, MinY(collidableTiles) - position.Y - height);
                        onGround = true;
                    }
                }
            }
        }

        private List<Rectangle> GetHitBoxesInRegion(Rectangle region, float depth, Direction direction)
        {
            List<Rectangle> hitboxes = new List<Rectangle>();

            // TODO: Don't hardcode in the map layer
            foreach(Map.TileLocation tileLocation in Map.GetTilesInRegion(region, 2))
            {
                ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                if (tile.HasComponent<Collidable>())
                {
                    Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                    if (tileBoundingRegion.Intersects(region) && !tile.GetComponent<Collidable>().ExcludedDirections.Contains(direction))
                        hitboxes.Add(tileBoundingRegion);
                }
            }

            return hitboxes;
        }

        private int MinX(List<Rectangle> tileHitBoxes)
        {
            int x = tileHitBoxes[0].Left;

            for (int i = 1; i < tileHitBoxes.Count; i++)
            {
                if (tileHitBoxes[i].Left < x)
                    x = tileHitBoxes[i].Left;
            }

            return x;
        }

        private int MaxX(List<Rectangle> tileHitBoxes)
        {
            int x = tileHitBoxes[0].Right;

            for (int i = 1; i < tileHitBoxes.Count; i++)
            {
                if (tileHitBoxes[i].Right > x)
                    x = tileHitBoxes[i].Left;
            }

            return x;
        }

        private int MinY(List<Rectangle> tileHitBoxes)
        {
            int y = tileHitBoxes[0].Top;

            for (int i = 1; i < tileHitBoxes.Count; i++)
            {
                if (tileHitBoxes[i].Top < y)
                    y = tileHitBoxes[i].Left;
            }

            return y;
        }

        private int MaxY(List<Rectangle> tileHitBoxes)
        {
            int y = tileHitBoxes[0].Bottom;

            for (int i = 1; i < tileHitBoxes.Count; i++)
            {
                if (tileHitBoxes[i].Bottom > y)
                    y = tileHitBoxes[i].Left;
            }

            return y;
        }
    }
}