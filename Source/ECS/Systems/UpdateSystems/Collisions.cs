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
        // TODO: Check whether map is null throughout
        protected Map? Map { get; private set; }

        public Collision(World world, Map? map = null) : base(world, typeof(Collidable), typeof(KinematicBody), typeof(Transform), typeof(Dimensions))
        {
            Map = map;
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            // TODO: Tidy this up

            Point vel = new Point((int)Math.Ceiling(components.GetComponent<KinematicBody>().Velocity.X), (int)Math.Ceiling(components.GetComponent<KinematicBody>().Velocity.Y));
            Point pos = components.GetComponent<Transform>().Position.ToPoint();
            float depth = components.GetComponent<Transform>().Depth;
            int width = components.GetComponent<Dimensions>().Width;
            int height = components.GetComponent<Dimensions>().Height;

            if (vel.X < 0)
            {
                Rectangle check = new Rectangle(pos.X + vel.X, pos.Y, -vel.X, height);
                ResolveCollision(check, depth, ref vel);
            }
            else if (vel.X > 0)
            {
                Rectangle check = new Rectangle(pos.X + width, pos.Y, vel.X, height);
                ResolveCollision(check, depth, ref vel);
            }

            if (vel.Y < 0)
            {
                Rectangle check = new Rectangle(pos.X, pos.Y + vel.Y, width, -vel.Y);
                ResolveCollision(check, depth, ref vel);
            }
            else if (vel.Y > 0)
            {
                Rectangle check = new Rectangle(pos.X, pos.Y + 1 + height - vel.Y, width, vel.Y);
                ResolveCollision(check, depth, ref vel);
            }

            components.GetComponent<KinematicBody>().Velocity = vel.ToVector2();
        }

        protected void ResolveCollision(Rectangle check, float depth, ref Point velocity)
        {
            foreach (ComponentCollection tile in Map.GetTilesInRegion(check, depth))
            {
                if (tile.HasComponent<Collidable>())
                {
                    Rectangle tileBoundingRegion = new Rectangle(tile.GetComponent<Collidable>().BoundingBox.Location + tile.GetComponent<Transform>().Position.ToPoint(), tile.GetComponent<Collidable>().BoundingBox.Size);

                    if (tileBoundingRegion.Intersects(check))
                    {
                        velocity = Point.Zero;

                        // TODO: Check if another solid block and move player to the closest block
                    }
                }
            }
        }
    }
}