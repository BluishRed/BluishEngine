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
    public class PassiveAnimation : UpdateSystem
    {
        public PassiveAnimation(World world) : base(world, typeof(PassivelyAnimated), typeof(Sprite))
        {

        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            components.GetComponent<PassivelyAnimated>().Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (components.GetComponent<PassivelyAnimated>().Timer >= components.GetComponent<PassivelyAnimated>().Frames[components.GetComponent<PassivelyAnimated>().CurrentFrame].duration)
            {
                components.GetComponent<PassivelyAnimated>().CurrentFrame++;
                components.GetComponent<PassivelyAnimated>().CurrentFrame %= components.GetComponent<PassivelyAnimated>().Frames.Length;
                components.GetComponent<PassivelyAnimated>().Timer = 0;
                components.GetComponent<Sprite>().Source = components.GetComponent<PassivelyAnimated>().Frames[components.GetComponent<PassivelyAnimated>().CurrentFrame].source;
            }
        }
    }
}