using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine
{
    public abstract class BluishState : State
    {
        public Camera Camera { get; set; }
        public Map Map { get; set; }

        public BluishState()
        {
            Camera = new Camera();
        }

        public void AddMap(string location)
        {
            Map = new Map(location, Camera);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, Camera.Transform());
            if (Map is not null)
                Map.Draw(spriteBatch);
            base.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (Map is not null)
                Map.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            if (Map is not null)
                Map.LoadContent(Content);
            base.LoadContent();
        }
    }
}