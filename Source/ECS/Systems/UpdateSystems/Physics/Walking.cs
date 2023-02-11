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
        public Walking(World world) : base(world, typeof(KinematicBody), typeof(CanWalk))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            // TODO: Make 'walking' in the air defined better
            if (components.GetComponent<KinematicBody>().CanMove)
            {
                if (Input.IsKeyInState(components.GetComponent<CanWalk>().LeftControls))
                {
                    components.GetComponent<KinematicBody>().Force.X -= components.GetComponent<CanWalk>().Force;
                }
                else if (Input.IsKeyInState(components.GetComponent<CanWalk>().RightControls))
                {
                    components.GetComponent<KinematicBody>().Force.X += components.GetComponent<CanWalk>().Force;
                }
            }
        }
    }
}