using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class MoveEntity : UpdateSystem
    {
        public MoveEntity(World world) : base(world, typeof(KinematicBody), typeof(Transform))
        {

        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            components.GetComponent<Transform>().Position += components.GetComponent<KinematicBody>().Velocity;
        }
    }
}