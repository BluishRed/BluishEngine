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
        public List<PointLight> Lights { get; set; }

        private RenderTarget2D _renderTarget;
        private RenderTarget2D _lightBuffer;

        public BluishState()
        {
            Camera = new Camera(Graphics.GameResolution);
            _renderTarget = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);
            _lightBuffer = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);
            Lights = new List<PointLight>();
        }
        
        public void AddMap(string location)
        {
            Map = new Map(location, Camera);
        }

        public override void PreRenderTargetDraw(SpriteBatch spriteBatch)
        {
            Graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
            Graphics.GraphicsDevice.Clear(Map is null ? Color.Black : new Color(Map.BackgroundColor, 0));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.PointClamp, null, null, Effects.GetEffect("Transparency"), Camera.Transform());
            Map?.Draw(spriteBatch);
            base.Draw(spriteBatch);
            spriteBatch.End();

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Lighting();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Effects.GetEffect("Lighting"), null);
            spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            Lights.Clear();
            Map?.Update(gameTime);
            Camera.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            Map?.LoadContent(Content);
            base.LoadContent();
            Effects.GetEffect("Lighting").Parameters["screenAspect"].SetValue((float)Graphics.GameResolution.X / Graphics.GameResolution.Y);
        }

        private void Lighting()
        {
            foreach (PointLight light in Lights)
            {
                Vector2 screenPosition = Vector2.Transform(light.Position, Camera.Transform());
                Effects.GetEffect("Lighting").Parameters["lightPosition"].SetValue(new Vector2(screenPosition.X / Graphics.GameResolution.X, screenPosition.Y / Graphics.GameResolution.Y));
                Effects.GetEffect("Lighting").Parameters["lightDepth"].SetValue(light.Depth);
                Effects.GetEffect("Lighting").Parameters["lightRadius"].SetValue(light.Radius * Camera.Zoom / Graphics.GameResolution.X);
            }
        }
    }
}

