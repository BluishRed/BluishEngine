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
        private const float _gravity = 0.3f;

        public Physics(World world) : base(world, typeof(KinematicBody), typeof(KinematicState))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            components.GetComponent<KinematicBody>().Force += new Vector2(0, _gravity * components.GetComponent<KinematicBody>().Mass);
            components.GetComponent<KinematicBody>().Velocity += components.GetComponent<KinematicBody>().Force / components.GetComponent<KinematicBody>().Mass;
            Vector2 vel = components.GetComponent<KinematicBody>().Velocity;
            vel.X *= 0.85f;
            components.GetComponent<KinematicBody>().Velocity = vel;
            components.GetComponent<KinematicBody>().Force = Vector2.Zero;
        }
    }
}