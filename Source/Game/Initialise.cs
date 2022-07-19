using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public abstract partial class BluishGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private int _scale;
        private RenderTarget2D _gameScreen;
        private SpriteBatch _spriteBatch;
        
        public BluishGame(BluishGameParameters gameParameters)
        {
            _graphics = new GraphicsDeviceManager(this);
            Graphics.GameResolution = gameParameters.Dimensions;
            Graphics.ScreenResolution = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            StateManager.SetInitialState(gameParameters.InitialState);
            Content.RootDirectory = "Content";
            ContentProvider.Content = Content;
            //_graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
        }

        protected sealed override void Initialize()
        {
            _scale = Math.Min(Graphics.ScreenResolution.Y / Graphics.GameResolution.Y, Graphics.ScreenResolution.X / Graphics.GameResolution.X);
            if (!_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = Graphics.GameResolution.X * _scale;
                _graphics.PreferredBackBufferHeight = Graphics.GameResolution.Y * _scale;
                _graphics.ApplyChanges();
            }
            _gameScreen = new RenderTarget2D(GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);

            StateManager.Initialise();

            base.Initialize();
        }

        protected sealed override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
    }
}