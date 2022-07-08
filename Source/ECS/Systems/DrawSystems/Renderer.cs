using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Systems
{
    public class Renderer : DrawSystem
    {
        public Renderer(World world) : base(world, typeof(Components.Sprite), typeof(Components.Transform))
        {
        }

        protected override void DrawEntity(SpriteBatch spriteBatch, Entity entity, ComponentCollection components)
        {
            spriteBatch.Draw(components.GetComponent<Components.Sprite>().Texture, components.GetComponent<Components.Transform>().Position, components.GetComponent<Components.Sprite>().Source, Color.White);
        }
    }
}