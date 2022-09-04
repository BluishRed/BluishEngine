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
            //components.GetComponent<KinematicBody>().Velocity = new Vector2((float)Math.Round(components.GetComponent<KinematicBody>().Velocity.X * 2, MidpointRounding.AwayFromZero) / 2, (float)Math.Round(components.GetComponent<KinematicBody>().Velocity.Y * 2, MidpointRounding.AwayFromZero) / 2);
            components.GetComponent<Transform>().Position += components.GetComponent<KinematicBody>().Velocity;
        }
    }
}