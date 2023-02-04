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

        private RenderTarget2D _renderTarget;

        public BluishState()
        {
            Camera = new Camera(Graphics.GameResolution);
            _renderTarget = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);
        }
        
        public void AddMap(string location)
        {
            Map = new Map(location, Camera);
        }

        public override void PreRenderTargetDraw(SpriteBatch spriteBatch)
        {
            Graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
            Graphics.GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.PointClamp, null, null, Effects.GetEffect("Transparency"), Camera.Transform());
            Map?.Draw(spriteBatch);
            base.Draw(spriteBatch);
            spriteBatch.End();

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            Map?.Update(gameTime);
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            Map?.LoadContent(Content);
            base.LoadContent();
        }
    }
}