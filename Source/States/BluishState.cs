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

        private RenderTarget2D _sceneRender;
        private RenderTarget2D _lightBuffer;
        private Texture2D _light;
        private BlendState _lightBlendState;
        private Random _random;

        public BluishState()
        {
            Camera = new Camera(Graphics.GameResolution);
            _sceneRender = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);
            _lightBuffer = new RenderTarget2D(Graphics.GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);
            Lights = new List<PointLight>();
            _random = new Random();
            _lightBlendState = new BlendState()
            {
                AlphaBlendFunction = BlendFunction.Max,
                ColorBlendFunction = BlendFunction.Max,
                AlphaSourceBlend = Blend.One,
                ColorSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.InverseDestinationAlpha,
                ColorDestinationBlend = Blend.InverseDestinationColor
            };
        }
        
        public void AddMap(string location)
        {
            Map = new Map(location, Camera);
        }

        public override void PreRenderTargetDraw(SpriteBatch spriteBatch)
        {
            Graphics.GraphicsDevice.SetRenderTarget(_sceneRender);
            Graphics.GraphicsDevice.Clear(Map is null ? Color.Black : new Color(Map.BackgroundColor, 0));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.PointClamp, null, null, Effects.GetEffect("Transparency"), Camera.Transform());
            Map?.Draw(spriteBatch);
            base.Draw(spriteBatch);
            spriteBatch.End();

            Graphics.GraphicsDevice.SetRenderTarget(null);

            Lighting(spriteBatch);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Effects.GetEffect("FadePalette").Parameters["LightBuffer"].SetValue(_lightBuffer);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, Effects.GetEffect("FadePalette"), null);
            //spriteBatch.Draw(_lightBuffer, Vector2.Zero, Color.White);
            spriteBatch.Draw(_sceneRender, Vector2.Zero, Color.White);
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
            _light = new Texture2D(Graphics.GraphicsDevice, 1, 1);
            _light.SetData(new Color[] { Color.Transparent });
            Map?.LoadContent(Content);
            base.LoadContent();
            Effects.GetEffect("FadePalette").Parameters["FadePalette"].SetValue(Effects.FadePalette);
        }

        private void Lighting(SpriteBatch spriteBatch)
        {
            Lights.AddRange(Map.Lights);

            Graphics.GraphicsDevice.SetRenderTarget(_lightBuffer);

            Graphics.GraphicsDevice.Clear(Color.White * Map.GetRoomContainingVector(Camera.Viewport.Center).AmbientLight);

            spriteBatch.Begin(SpriteSortMode.Deferred, _lightBlendState, null, null, null, Effects.GetEffect("Lighting"), Camera.Transform());
            foreach (PointLight light in Lights)
            {
                Vector2 lightPosition = light.Position - new Vector2(light.Radius / 2);
                //Effects.GetEffect("Lighting").Parameters["Scene"].SetValue(_sceneRender);
                //Effects.GetEffect("Lighting").Parameters["SceneLocation"].SetValue(new Vector2(lightPosition.X / _sceneRender.Width, lightPosition.Y / _sceneRender.Height));
                Effects.GetEffect("Lighting").Parameters["Brightness"].SetValue(light.Brightness);
                //Effects.GetEffect("Lighting").Parameters["Random"].SetValue(0.213802358f);
                Effects.GetEffect("Lighting").Parameters["Position"].SetValue(Vector2.Floor(lightPosition));
                spriteBatch.Draw(_light, lightPosition, null, Color.White, 0f, Vector2.Zero, light.Radius, SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            Graphics.GraphicsDevice.SetRenderTarget(null);
        }
    }
}