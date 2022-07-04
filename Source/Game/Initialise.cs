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
        public Dimensions Resolution { get; private set; }

        private GraphicsDeviceManager _graphics;
        private int _scale;
        private RenderTarget2D _gameScreen;
        private SpriteBatch _spriteBatch;
        
        /// <param name="resolution">
        /// The resolution of the game
        /// </param>
        public BluishGame(BluishGameParameters gameParameters)
        {
            _graphics = new GraphicsDeviceManager(this);
            Resolution = gameParameters.Dimensions;
            StateManager.SetInitialState(gameParameters.InitialState);
            Content.RootDirectory = "Content";
            ContentProvider.Content = Content;
        }

        protected sealed override void Initialize()
        {
            _scale = Math.Min(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / Resolution.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / Resolution.Width);
            if (!_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = Resolution.Width * _scale;
                _graphics.PreferredBackBufferHeight = Resolution.Height * _scale;
                _graphics.ApplyChanges();
            }
            _gameScreen = new RenderTarget2D(GraphicsDevice, Resolution.Width, Resolution.Height);

            StateManager.Initialise();

            base.Initialize();
        }

        protected sealed override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
    }
}