using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class Jumping : UpdateSystem
    {
        public Jumping(World world) : base(world, typeof(KinematicBody), typeof(CanJump), typeof(KinematicState))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (Input.IsKeyInState(components.GetComponent<CanJump>().Controls) && components.GetComponent<KinematicState>().OnGround)
            {
                components.GetComponent<KinematicBody>().Force.Y -= components.GetComponent<CanJump>().Force;
            }
        }
    }
}