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
            Camera = new Camera(Graphics.GameResolution);
        }

        public void AddMap(string location)
        {
            Map = new Map(location, Camera);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Implement the depth

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.Transform());
            if (Map is not null)
                Map.Draw(spriteBatch);
            base.Draw(spriteBatch);
            spriteBatch.End();
        }
        
        public override void Update(GameTime gameTime)
        {
            if (Map is not null)
                Map.Update(gameTime);

            // TODO: Remove this eventually

            if (Input.IsKeyJustPressed(Keys.W))
            {
                Camera.Zoom *= 2f;
            }
            if (Input.IsKeyJustPressed(Keys.S))
            {
                Camera.Zoom *= 0.5f;
            }

            base.Update(gameTime);

            // TODO: Move this

            if (Map is not null)
                Camera.ClampViewport(0, Map.Dimensions.X, 0, Map.Dimensions.Y);
        }

        public override void LoadContent()
        {
            if (Map is not null)
                Map.LoadContent(Content);
            base.LoadContent();
        }
    }
}