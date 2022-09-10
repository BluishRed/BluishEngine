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
    public class Walking : UpdateSystem
    {
        public Walking(World world) : base(world, typeof(KinematicBody), typeof(CanWalk), typeof(KinematicState))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            // TODO: Make 'walking' in the air defined better

            Vector2 force = components.GetComponent<KinematicBody>().Force;

            if (Input.IsKeyInState(components.GetComponent<CanWalk>().LeftControls))
            {
                force.X -= components.GetComponent<CanWalk>().Force;
            }
            else if (Input.IsKeyInState(components.GetComponent<CanWalk>().RightControls))
            {
                force.X += components.GetComponent<CanWalk>().Force;
            }

            components.GetComponent<KinematicBody>().Force = force;
        }
    }
}