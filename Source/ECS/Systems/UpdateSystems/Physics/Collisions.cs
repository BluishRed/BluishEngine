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

        public Collision(World world, Map? map = null) : base(world, typeof(Collidable), typeof(KinematicBody), typeof(Transform), typeof(KinematicState))
        {
            Map = map;
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        { 
            Vector2 pos = components.GetComponent<Transform>().Position + components.GetComponent<Collidable>().BoundingBox.Location.ToVector2();
            Vector2 vel = components.GetComponent<KinematicBody>().Velocity;
            bool onGround = components.GetComponent<KinematicState>().OnGround;

            // TODO: If collision is sideways then it won't work
            if (Map is not null)
            {
                ResolveMapCollisions(
                    components.GetComponent<Collidable>().BoundingBox.Width,
                    components.GetComponent<Collidable>().BoundingBox.Height,
                    components.GetComponent<Transform>().Depth,
                    ref pos,
                    ref vel,
                    ref onGround
                );
            }
            
            components.GetComponent<Transform>().Position = pos - components.GetComponent<Collidable>().BoundingBox.Location.ToVector2();
            components.GetComponent<KinematicBody>().Velocity = vel;
            components.GetComponent<KinematicState>().OnGround = onGround;
        }

        // TODO: Tidy this up
        protected void ResolveMapCollisions(int width, int height, float depth, ref Vector2 pos, ref Vector2 vel, ref bool onGround)
        {
            if (vel.X != 0)
            {
                Rectangle check;

                if (vel.X < 0)
                {
                    check = new Rectangle((int)(pos.X + vel.X), (int)pos.Y, (int)Math.Ceiling(-vel.X), height);

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth);

                    if (collidableTiles.Count > 0)
                    {
                        vel.X = 0;
                        pos.X += Math.Min(0, MaxX(collidableTiles) - pos.X);
                    }
                }
                else
                {
                    check = new Rectangle((int)pos.X + width, (int)pos.Y, (int)Math.Ceiling(vel.X), height);

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth);

                    if (collidableTiles.Count > 0)
                    {
                        vel.X = 0;
                        pos.X += Math.Max(0, MinX(collidableTiles) - pos.X - width);
                    }
                }
            }

            onGround = false;

            if (vel.Y != 0)
            {
                Rectangle check;

                if (vel.Y < 0)
                {
                    check = new Rectangle((int)pos.X, (int)(pos.Y + vel.Y), width, (int)Math.Ceiling(-vel.Y));

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth);

                    if (collidableTiles.Count > 0)
                    {
                        vel.Y = 0;
                        pos.Y += Math.Min(0, MaxY(collidableTiles) - pos.Y);
                    }                
                }
                else
                {
                    check = new Rectangle((int)pos.X, (int)(pos.Y + height + vel.Y), width, (int)Math.Ceiling(vel.Y));

                    List<Rectangle> collidableTiles = GetHitBoxesInRegion(check, depth);

                    if (collidableTiles.Count > 0)
                    {
                        vel.Y = 0;
                        pos.Y += Math.Max(0, MinY(collidableTiles) - pos.Y - height);
                        onGround = true;
                    }
                }
            }
        }

        private List<Rectangle> GetHitBoxesInRegion(Rectangle region, float depth)
        {
            List<Rectangle> hitboxes = new List<Rectangle>();

            foreach(Map.TileLocation tileLocation in Map.GetTilesInRegion(region, 2))
            {
                ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                if (tile.HasComponent<Collidable>())
                {
                    Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                    if (tileBoundingRegion.Intersects(region))
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