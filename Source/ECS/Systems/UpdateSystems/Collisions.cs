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
            Point vel = new Point((int)Math.Ceiling(components.GetComponent<KinematicBody>().Velocity.X), (int)Math.Ceiling(components.GetComponent<KinematicBody>().Velocity.Y));
            Point pos = components.GetComponent<Transform>().Position.ToPoint();
            float depth = components.GetComponent<Transform>().Depth;
            int width = components.GetComponent<Dimensions>().Width;
            int height = components.GetComponent<Dimensions>().Height;

            if (vel.X < 0)
            {
                Rectangle check = new Rectangle(pos.X - vel.X, pos.Y, vel.X, height);
            }
            else if (vel.X > 0)
            {
                
            }

            if (vel.Y > 0)
            {

            }
            else if (vel.Y < 0)
            {

            }
        }
    }
}