using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using System.Runtime.InteropServices;

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
                if (Bounds.HasValue)
                    _zoom = Math.Max(value, Math.Max((float)_defaultDimensions.X / Bounds.Value.Width, (float)_defaultDimensions.Y / Bounds.Value.Height));
                else
                    _zoom = value;
            }
        }
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
                }
            }
        }

        private bool _canManuallyMove;
        private Vector2 _position;
        private float _zoom;
        private Rectangle? _bounds;
        private Point _defaultDimensions;
        private List<CameraEffect> _effects;
        private List<CameraEffect> _effectsToRemove;

        /// <param name="viewportDimensions">
        /// <inheritdoc cref="ViewportDimensions" path="/summary"/>
        /// </param>
        public Camera(Point defaultViewportDimensions)
        {
            _defaultDimensions = defaultViewportDimensions;
            _effects = new List<CameraEffect>();
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
            Position = new Vector2(centre.X - Viewport.Size.X / 2, centre.Y - Viewport.Size.Y / 2);
        }

        public void Update(GameTime gameTime)
        {
            foreach (CameraEffect effect in _effects)
            {
                effect.Update(gameTime);

                if (effect.Completed)
                    _effectsToRemove.Add(effect);
            }

            foreach(CameraEffect effect in _effectsToRemove)
            {
                _effects.Remove(effect);
            }
        }

        public void SlideTo(Vector2 destination, float duration)
        {
            _effects.Add(new Swipe(this, destination, duration));
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

        class Swipe : CameraEffect
        {
            private Vector2 _destination;

            public Swipe(Camera camera, Vector2 destination, float duration) : base(camera, duration)
            {
                _destination = destination;
                camera._canManuallyMove = false;
            }

            public override void Update(GameTime gameTime)
            {
                Camera._position = Vector2.Lerp(Camera.Position, _destination, ElapsedTime / Duration);
                base.Update(gameTime);
            }
        }
    }
}