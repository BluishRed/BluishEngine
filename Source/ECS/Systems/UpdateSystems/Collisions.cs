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
                Point vel = new Point((int)Math.Ceiling(components.GetComponent<KinematicBody>().Velocity.X), (int)Math.Ceiling(components.GetComponent<KinematicBody>().Velocity.Y));
                Point pos = components.GetComponent<Transform>().Position.ToPoint();
                float depth = components.GetComponent<Transform>().Depth;
                int width = components.GetComponent<Dimensions>().Width;
                int height = components.GetComponent<Dimensions>().Height;

                // TODO: If collision is sideways then it won't work

                if (vel.X < 0)
                {
                    Rectangle check = new Rectangle(pos.X + vel.X, pos.Y, -vel.X, height);
                    if (WillCollide(check, depth)) vel.X = 0;
                }
                else if (vel.X > 0)
                {
                    Rectangle check = new Rectangle(pos.X + width, pos.Y, vel.X, height);
                    if (WillCollide(check, depth)) vel.X = 0;
                }

                if (vel.Y < 0)
                {
                    Rectangle check = new Rectangle(pos.X, pos.Y + vel.Y, width, -vel.Y);
                    if (WillCollide(check, depth)) vel.Y = 0;
                }
                else if (vel.Y > 0)
                {
                    Rectangle check = new Rectangle(pos.X, pos.Y + height, width, vel.Y);
                    if (WillCollide(check, depth)) vel.Y = 0;
                }

                components.GetComponent<KinematicBody>().Velocity = vel.ToVector2();
            }
            // TODO: Tidy this up  
        }

        protected bool WillCollide(Rectangle check, float depth)
        {

            if (!Map.Bounds.Contains(check)) return true;

            // TODO: Use depth
            foreach (ComponentCollection tile in Map.GetTilesInRegion(check, 2))
            {
                if (tile.HasComponent<Collidable>())
                {
                    Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tile.GetComponent<Transform>().Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                    return tileBoundingRegion.Intersects(check);
                }
            }

            return false;
        }
    }
}