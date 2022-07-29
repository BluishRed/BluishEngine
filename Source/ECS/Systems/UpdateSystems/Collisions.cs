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

        public Collision(World world, Map? map = null) : base(world, typeof(Collidable), typeof(KinematicBody), typeof(Transform), typeof(Dimensions))
        {
            Map = map;
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (Map is not null)
            {
                Vector2 vel = components.GetComponent<KinematicBody>().Velocity;
                Point absVel = new Point((int)Math.Ceiling(Math.Abs(vel.X)), (int)Math.Ceiling(Math.Abs(vel.Y)));
                Point pos = components.GetComponent<Transform>().Position.ToPoint();
                float depth = components.GetComponent<Transform>().Depth;
                int width = components.GetComponent<Dimensions>().Width;
                int height = components.GetComponent<Dimensions>().Height;

                // TODO: If collision is sideways then it won't work

                if (vel.X < 0)
                {
                    Rectangle check = new Rectangle(pos.X - absVel.X, pos.Y, absVel.X, height);
                    if (WillCollide(check, depth)) vel.X = 0;
                }
                else if (vel.X > 0)
                {
                    Rectangle check = new Rectangle(pos.X + width, pos.Y, absVel.X, height);
                    if (WillCollide(check, depth)) vel.X = 0;
                }

                if (vel.Y < 0)
                {
                    Rectangle check = new Rectangle(pos.X, pos.Y - absVel.Y, width, absVel.Y);
                    if (WillCollide(check, depth)) vel.Y = 0;
                }
                else if (vel.Y > 0)
                {
                    Rectangle check = new Rectangle(pos.X, pos.Y + height, width, absVel.Y);
                    if (WillCollide(check, depth)) vel.Y = 0;
                }

                components.GetComponent<KinematicBody>().Velocity = vel;
            }
            // TODO: Tidy this up
        }

        protected bool WillCollide(Rectangle check, float depth)
        {

            if (!Map.Bounds.Contains(check)) return true;

            // TODO: Use depth
            foreach (Map.TileLocation tileLocation in Map.GetTilesInRegion(check, 2))
            {
                ComponentCollection tile = Map.GetComponents(tileLocation.Tile);
                if (tile.HasComponent<Collidable>())
                {
                    Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tileLocation.Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                    return tileBoundingRegion.Intersects(check);
                }
            }

            return false;
        }
    }
}