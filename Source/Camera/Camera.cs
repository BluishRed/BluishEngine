using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using System.Diagnostics;

namespace BluishEngine
{
    /// <summary>
    /// Wrapper function for a <see cref="Matrix"/> that transforms world coordinates to screen coordinates
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// A <see cref="float"/> representing the zoom level, with <c>1</c> being the default zoom
        /// </summary>
        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _position -= new Vector2(Viewport.Width / 2 * (1 - value/_zoom), Viewport.Height / 2 * (1 - value/_zoom));

                if (Bounds.HasValue)
                    _zoom = Math.Max(value, Math.Max((float)_defaultDimensions.X / Bounds.Value.Width, (float)_defaultDimensions.Y / Bounds.Value.Height));
                else
                    _zoom = value;
            }
        }
        /// <summary>
        /// The location of the top-left corner of the viewport
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_canManuallyMove)
                {
                    _position = value;
                    ClampViewportToBounds();
                }  
            }
        }
        /// <summary>
        /// The viewable area of the world as a <see cref="Rectangle"/>
        /// </summary>
        public Rectangle Viewport
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Math.Ceiling(_defaultDimensions.X / Zoom), (int)Math.Ceiling(_defaultDimensions.Y / Zoom));
            }
            set
            {
                Zoom = value.Width / _defaultDimensions.X;
                Position = value.Location.ToVector2();
                ClampViewportToBounds();
            }
        }
        /// <summary>
        /// An optional <see cref="Rectangle"/> restricting the range of movement of this <see cref="Camera"/>
        /// </summary>
        public Rectangle? Bounds { 
            get
            {
                return _bounds;
            }
            set
            {
                if (_canManuallyMove)
                {
                    _bounds = value;
                    ClampViewportToBounds();
                }
            }
        }

        private bool _canManuallyMove;
        private Vector2 _position;
        private float _zoom;
        private Rectangle? _bounds;
        private Point _defaultDimensions;
        private Dictionary<Type, CameraEffect> _effects;
        private List<CameraEffect> _effectsToRemove;

        /// <param name="viewportDimensions">
        /// <inheritdoc cref="ViewportDimensions" path="/summary"/>
        /// </param>
        public Camera(Point defaultViewportDimensions)
        {
            _defaultDimensions = defaultViewportDimensions;
            _effects = new Dictionary<Type, CameraEffect>();
            _effectsToRemove = new List<CameraEffect>();
            _canManuallyMove = true;
            Viewport = new Rectangle(Point.Zero, _defaultDimensions);
        }

        /// <summary>
        /// Encapsulates this <see cref="Camera"/> as a <see cref="Matrix"/>
        /// </summary>
        /// <returns>
        /// A <see cref="Matrix"/> that transforms world coordinates to screen coordinates
        /// </returns>
        public Matrix Transform()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
                * Matrix.CreateScale(Zoom, Zoom, 1);
        }
        
        /// <summary>
        /// Sets <paramref name="centre"/> to the middle of the <see cref="Viewport"/>
        /// </summary>
        public void FocusOn(Vector2 centre)
        {
            Position = centre - Viewport.Size.ToVector2() / 2;
        }
        
        public void SmoothFocusOn(GameTime gameTime, Vector2 centre, float smoothing, Vector2 velocity, Vector2 acceleration)
        {
            Vector2 position = Position;

            if (Math.Abs(acceleration.X) == 0 && velocity.X != 0)
            {
                position.X += velocity.X;
            }
            else
            {
                position.X = MathHelper.SmoothStep(position.X, centre.X - Viewport.Width / 2, 1 - (float)Math.Pow(smoothing, gameTime.ElapsedGameTime.TotalSeconds * 25));position.X = MathHelper.SmoothStep(position.X, centre.X - Viewport.Width / 2, 1 - (float)Math.Pow(smoothing, gameTime.ElapsedGameTime.TotalSeconds * 25));
            }
            if (Math.Abs(acceleration.Y) == 0 && velocity.Y != 0)
            {
                position.Y += velocity.Y;
            }
            else
            {
                position.Y = MathHelper.SmoothStep(position.Y, centre.Y - Viewport.Height / 2, 1 - (float)Math.Pow(smoothing, gameTime.ElapsedGameTime.TotalSeconds * 25));
            }

            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            foreach (CameraEffect effect in _effects.Values)
            {
                effect.Update(gameTime);

                if (effect.Completed)
                {
                    _effectsToRemove.Add(effect);
                }
            }

            foreach (CameraEffect effect in _effectsToRemove)
            {
                if (_effects[effect.GetType()] == effect)
                {
                    _effects.Remove(effect.GetType());
                }
            }

            _effectsToRemove.Clear();
        }

        public void SlideTo(Vector2 destination, float duration)
        {
            _effects[typeof(Pan)] = new Pan(this, destination, duration);
        }

        public void ZoomBy(float factor, float duration)
        {
            _effects[typeof(SmoothZoom)] = new SmoothZoom(this, factor, duration);
        }

        private void ClampViewportToBounds()
        {
            if (Bounds.HasValue)
            {
                _position.X = Math.Clamp(_position.X, Bounds.Value.Left, Bounds.Value.Right - Viewport.Width);
                _position.Y = Math.Clamp(_position.Y, Bounds.Value.Top, Bounds.Value.Bottom - Viewport.Height);
            }
        }

        class CameraEffect
        {
            public bool Completed { get; private set; }
            protected float Duration { get; private set; }
            protected float ElapsedTime { get; private set; }
            protected Camera Camera { get; private set; }

            public CameraEffect(Camera camera, float duration)
            {
                Duration = duration;
                ElapsedTime = 0;
                Camera = camera;
            }

            public virtual void Update(GameTime gameTime)
            {
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ElapsedTime >= Duration)
                {
                    Completed = true;
                    Camera._canManuallyMove = true;
                }
            }
        }

        class Pan : CameraEffect
        {
            private Vector2 _destination;

            public Pan(Camera camera, Vector2 destination, float duration) : base(camera, duration)
            {
                _destination = destination;
            }

            public override void Update(GameTime gameTime)
            {
                Camera._canManuallyMove = false;
                Camera._position = Vector2.SmoothStep(Camera.Position, _destination, MathHelper.SmoothStep(0, 1, ElapsedTime / Duration));
                Camera._position.Round();
                base.Update(gameTime);
            }
        }

        class SmoothZoom : CameraEffect
        {
            private float _factor;
            private float _initialZoom;

            public SmoothZoom(Camera camera, float factor, float duration) : base(camera, duration)
            {
                _factor = factor;
                _initialZoom = camera.Zoom;
            }

            public override void Update(GameTime gameTime)
            {
                Camera.Zoom = MathHelper.SmoothStep(Camera.Zoom, _initialZoom * _factor, MathHelper.SmoothStep(0, 1, ElapsedTime / Duration));
                base.Update(gameTime);
            }
        }
    }
}