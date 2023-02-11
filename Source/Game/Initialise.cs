using System;
using System.IO;
using System.Diagnostics;
using System.Resources;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using Microsoft.Xna.Framework.Content;

namespace BluishEngine
{
    public abstract partial class BluishGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private int _scale;
        private RenderTarget2D _gameScreen;
        private SpriteBatch _spriteBatch;
        private string _fadePaletteLocation;

        public BluishGame(BluishGameParameters gameParameters)
        {
            _graphics = new GraphicsDeviceManager(this);
            _fadePaletteLocation = gameParameters.FadePaletteLocation;
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
            _graphics.PreferredBackBufferWidth = Graphics.ScreenResolution.X;
            _graphics.PreferredBackBufferHeight = Graphics.ScreenResolution.Y;
            _graphics.ApplyChanges();
            Graphics.GraphicsDevice = GraphicsDevice;

            _gameScreen = new RenderTarget2D(GraphicsDevice, Graphics.GameResolution.X, Graphics.GameResolution.Y);

            base.Initialize();
        }

        protected sealed override void LoadContent()
        {
            Effects.LoadAssets(Content, _fadePaletteLocation);
            StateManager.Initialise();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }
    }
}