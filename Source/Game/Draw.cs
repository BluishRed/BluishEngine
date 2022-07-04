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
            GraphicsDevice.SetRenderTarget(_gameScreen);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            StateManager.CurrentState.Draw(_spriteBatch);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            _spriteBatch.Draw(_gameScreen, Vector2.Zero, null, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
            _spriteBatch.End();
        }
    }
}