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
        public Renderer(World world) : base(world, typeof(Sprite), typeof(Transform))
        {
        }

        protected override void DrawEntity(SpriteBatch spriteBatch, Entity entity, ComponentCollection components)
        {
            spriteBatch.Draw(components.GetComponent<Sprite>().Texture, components.GetComponent<Transform>().Position, components.GetComponent<Sprite>().Source, Color.White, components.GetComponent<Transform>().Rotation, Vector2.Zero, components.GetComponent<Transform>().Scale, SpriteEffects.None, components.GetComponent<Transform>().Depth);
        }
    }
}