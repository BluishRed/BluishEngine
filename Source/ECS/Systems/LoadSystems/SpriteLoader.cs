using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Systems
{

    // TODO: Automate the adding of loading systems and make internal
    public class SpriteLoader : LoadSystem
    {
        public SpriteLoader(World world) : base(world, typeof(Components.Sprite))
        {
        }

        protected override void LoadEntity(ContentManager content, Entity entity, ComponentCollection components)
        {
            components.GetComponent<Components.Sprite>().Texture = content.Load<Texture2D>(components.GetComponent<Components.Sprite>().Location);
        }
    }
}