using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class Physics : UpdateSystem
    {
        private const float _gravity = 0.4f;

        public Physics(World world) : base(world, typeof(KinematicBody), typeof(KinematicState))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<KinematicBody>().CanMove)
            {
                // TODO: Tidy up
                Vector2 previousVelocity = components.GetComponent<KinematicBody>().Velocity;
                components.GetComponent<KinematicBody>().Force += new Vector2(0, _gravity * components.GetComponent<KinematicBody>().Mass);
                components.GetComponent<KinematicBody>().Velocity += components.GetComponent<KinematicBody>().Force / components.GetComponent<KinematicBody>().Mass;

                Vector2 velocity = components.GetComponent<KinematicBody>().Velocity;

                velocity.X *= 0.85f;

                if (Math.Abs(velocity.X) < 0.1f)
                {
                    velocity.X = 0;
                }
                if (Math.Abs(velocity.Y) < 0.1f)
                {
                    velocity.Y = 0;
                }

                components.GetComponent<KinematicBody>().Velocity = velocity;

                Vector2 acceleration = velocity - previousVelocity;

                if (Math.Abs(acceleration.X) < 0.001f)
                {
                    acceleration.X = 0;
                }
                if (Math.Abs(acceleration.Y) < 0.001f)
                {
                    acceleration.Y = 0;
                }

                components.GetComponent<KinematicBody>().Acceleration = acceleration;
            }

            components.GetComponent<KinematicBody>().Force = Vector2.Zero;
        }
    }
}