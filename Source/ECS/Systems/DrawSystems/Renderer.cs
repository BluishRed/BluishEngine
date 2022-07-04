using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public class Renderer : DrawSystem
    {
        public Renderer(World world) : base(world, typeof(Sprite), typeof(Transform))
        {
        }

        protected override void DrawEntity(SpriteBatch spriteBatch, int entity, ComponentCollection components)
        {
            spriteBatch.Draw(components.GetComponent<Sprite>().Texture, components.GetComponent<Transform>().Position, Color.White);
        }
    }
}