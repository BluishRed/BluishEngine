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

        private const float _gravity = 2f;

        public Physics(World world) : base(world, typeof(Collidable), typeof(KinematicBody), typeof(Transform))
        {

        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            //components.GetComponent<Components.KinematicBody>().Force = new Vector2(0, _gravity);

            components.GetComponent<KinematicBody>().Velocity += components.GetComponent<KinematicBody>().Force / components.GetComponent<KinematicBody>().Mass;
            components.GetComponent<Transform>().Position += components.GetComponent<KinematicBody>().Velocity;
        }
    }
}