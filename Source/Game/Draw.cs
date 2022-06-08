using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BluishEngine
{
    partial class BluishGame
    {
        SpriteBatch _spritebatch;
        SpriteFont _ariel;
        RenderTarget2D _gameScreen;
        int _scale;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_gameScreen);
            _spritebatch.Begin(SpriteSortMode.FrontToBack);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            StateManager.CurrentState.Draw(_spritebatch);
            _spritebatch.End();

            GraphicsDevice.SetRenderTarget(null);
            _spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            GraphicsDevice.Clear(Color.Black);
            _spritebatch.Draw(
                texture: _gameScreen,
                position: new Vector2(GraphicsDevice.Viewport.Width % Resolution.Width / 2, GraphicsDevice.Viewport.Height % Resolution.Height / 2),
                sourceRectangle: null,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: _scale,
                effects: SpriteEffects.None,
                layerDepth: 0
            );
#if DEBUG
            PrintDebugInfo(gameTime, _spritebatch);
#endif
            _spritebatch.End();
        }

        private void PrintDebugInfo(GameTime gameTime, SpriteBatch spriteBatch)
        {
            PrintFPS(gameTime, spriteBatch);
        }

        private void PrintFPS(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_ariel, $"FPS:{Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds)}", Vector2.Zero, Color.White);
        }
    }
}