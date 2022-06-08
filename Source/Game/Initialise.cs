using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishEngine.Input;

namespace BluishEngine
{
    public abstract partial class BluishGame : Game
    {

        GraphicsDeviceManager _graphics;
        InputMap _commonInputMap;

        public static Dimensions Resolution { get; private set; }
        
        /// <param name="resolution">
        /// The resolution of the game
        /// </param>
        public BluishGame(Dimensions resolution)
        {
            _graphics = new GraphicsDeviceManager(this);
            Resolution = resolution;
            Content.RootDirectory = "Content";
        }

        protected sealed override void Initialize()
        {
            ContentLoader.CommonContent = Content;

            Init();

            _scale = Math.Min(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / Resolution.Height, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / Resolution.Width);
            if (!_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferWidth = Resolution.Width * _scale;
                _graphics.PreferredBackBufferHeight = Resolution.Height * _scale;
                _graphics.ApplyChanges();
            }
            _gameScreen = new RenderTarget2D(GraphicsDevice, Resolution.Width, Resolution.Height);

            _commonInputMap = new InputMap(
                (Keys.F11, KeyPressState.JustPressed, _graphics.ToggleFullScreen),
                (Keys.Escape, KeyPressState.JustPressed, Exit)
            );

            StateManager.Initialise();

            base.Initialize();
        }

        protected abstract void Init();

        protected sealed override void LoadContent()
        {
            _spritebatch = new SpriteBatch(GraphicsDevice);
            _ariel = Content.Load<SpriteFont>("Common/Fonts/Ariel");
        }
    }
}