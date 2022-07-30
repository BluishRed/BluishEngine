using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class Collision : UpdateSystem
    {
        protected Map? Map { get; private set; }

        public Collision(World world, Map? map = null) : base(world, typeof(Collidable), typeof(KinematicBody), typeof(Transform))
        {
            Map = map;
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            Vector2 vel = components.GetComponent<KinematicBody>().Velocity;

            // TODO: If collision is sideways then it won't work
            if (Map is not null)
            {
                ResolveMapCollisions(
                    components.GetComponent<Collidable>().BoundingBox.Width,
                    components.GetComponent<Collidable>().BoundingBox.Height,
                    components.GetComponent<Transform>().Depth,
                    components.GetComponent<Transform>().Position + components.GetComponent<Collidable>().BoundingBox.Location.ToVector2(),
                    ref vel
                );
            }

            components.GetComponent<KinematicBody>().Velocity = vel;
        }

        // TODO: Tidy this up
        protected void ResolveMapCollisions(int width, int height, float depth, Vector2 pos, ref Vector2 vel)
        {

            if (vel.X != 0)
            {
                Rectangle check;

                if (vel.X < 0)
                {
                    check = new Rectangle((int)(pos.X + vel.X), (int)pos.Y, (int)Math.Ceiling(-vel.X), height);

                    int x = -1;

                    foreach (Map.TileLocation tileLocation in Map.GetTilesInRegion(check, 2))
                    {
                        ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                        if (tile.HasComponent<Collidable>())
                        {
                            Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                            if (tileBoundingRegion.Intersects(check) && (tileBoundingRegion.Right > x || x == -1))
                                x = tileBoundingRegion.Right;
                        }
                    }

                    if (x > 0)
                        vel.X = Math.Min(0, x - pos.X);
                }
                else
                {
                    check = new Rectangle((int)pos.X + width, (int)pos.Y, (int)Math.Ceiling(vel.X), height);

                    int x = -1;

                    foreach (Map.TileLocation tileLocation in Map.GetTilesInRegion(check, 2))
                    {
                        ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                        if (tile.HasComponent<Collidable>())
                        {
                            Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                            if (tileBoundingRegion.Intersects(check) && (tileBoundingRegion.Left < x || x == -1))
                                x = tileBoundingRegion.Left;
                        }
                    }

                    if (x > 0)
                        vel.X = Math.Max(0, x - pos.X - width);
                }
            }

            if (vel.Y != 0)
            {
                Rectangle check;

                if (vel.Y < 0)
                {
                    check = new Rectangle((int)(pos.X), (int)(pos.Y + vel.Y), width, (int)-vel.Y);

                    int y = -1;

                    foreach (Map.TileLocation tileLocation in Map.GetTilesInRegion(check, 2))
                    {
                        ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                        if (tile.HasComponent<Collidable>())
                        {
                            Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                            if (tileBoundingRegion.Intersects(check) && (tileBoundingRegion.Bottom > y || y == -1)) 
                                y = tileBoundingRegion.Bottom;
                        }
                    }

                    if (y > 0)
                        vel.Y = Math.Min(0, pos.Y - y);
                }
                else
                {
                    check = new Rectangle((int)pos.X, (int)(pos.Y + height + vel.Y), width, (int)Math.Ceiling(vel.Y));

                    int y = -1;

                    foreach (Map.TileLocation tileLocation in Map.GetTilesInRegion(check, 2))
                    {
                        ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                        if (tile.HasComponent<Collidable>())
                        {
                            Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                            if (tileBoundingRegion.Intersects(check) && (tileBoundingRegion.Top < y || y == -1))
                                y = tileBoundingRegion.Top;
                        }
                    }

                    if (y > 0)
                        vel.Y = Math.Max(0, y - pos.Y - height);
                }
            }
        }
    }
}