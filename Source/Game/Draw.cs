using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    partial class BluishGame
    {
        protected override void Draw(GameTime gameTime)
        {
            StateManager.CurrentState.PreRenderTargetDraw(_spriteBatch);

            GraphicsDevice.SetRenderTarget(_gameScreen);
            StateManager.CurrentState.Draw(_spriteBatch);
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _spriteBatch.Draw(_gameScreen, new Vector2(0.5f * (Graphics.ScreenResolution.X - Graphics.GameResolution.X * _scale), 0.5f * (Graphics.ScreenResolution.Y - Graphics.GameResolution.Y * _scale)), null, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
            _spriteBatch.End();
        }
    }
}