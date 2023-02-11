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
    public class SpriteLoader : LoadSystem
    {
        public SpriteLoader(World world) : base(world, typeof(Sprite))
        {
        }

        protected override void LoadEntity(ContentManager content, Entity entity, ComponentCollection components)
        {
            components.GetComponent<Sprite>().Texture = content.Load<Texture2D>(components.GetComponent<Sprite>().Location);
        }
    }
}