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
    public class Renderer : DrawSystem
    {
        public Renderer(World world) : base(world, typeof(Components.Sprite), typeof(Transform))
        {
        }

        protected override void DrawEntity(SpriteBatch spriteBatch, Entity entity, ComponentCollection components)
        {
            spriteBatch.Draw(components.GetComponent<Components.Sprite>().Texture, components.GetComponent<Transform>().Position, components.GetComponent<Components.Sprite>().Source, Color.White);
        }
    }
}