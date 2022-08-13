using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;


namespace BluishEngine
{
    /// <summary>
    /// Wrapper function for a <see cref="Matrix"/> that transforms world coordinates to screen coordinates
    /// </summary>
    public class Camera
    {
        private Vector2 _focus;
        /// <summary>
        /// The center of the viewport
        /// </summary>
        public Vector2 Focus 
        { 
            get
            {
                return _focus;
            }
            set
            {
                _focus = value;
                ClampViewportToBounds();
            }
        }
        private float _zoom;

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
                    _zoom = Math.Max(value, Math.Max((float)PixelDimensions.X / Bounds.Value.Width, (float)PixelDimensions.Y / Bounds.Value.Height));
                else
                    _zoom = value;
            }
        }
        /// <summary>
        /// The viewable area of the world as a <see cref="Rectangle"/>
        /// </summary>
        public Rectangle Viewport
        {
            get
            {
                int width = (int)Math.Ceiling(PixelDimensions.X / Zoom);
                int height = (int)Math.Ceiling(PixelDimensions.Y / Zoom);
                return new Rectangle((int)(Focus.X - Math.Ceiling(width / 2f)), (int)(Focus.Y - (int)Math.Ceiling(height / 2f)), width, height);
            }
            set
            {
                Focus = value.Center.ToVector2();
                Zoom = (float)PixelDimensions.Y / value.Height;
                ClampViewportToBounds();
            }
        }
        /// <summary>
        /// An optional <see cref="Rectangle"/> restricting the range of movement of this <see cref="Camera"/>
        /// </summary>
        public Rectangle? Bounds { get; set; }
        /// <summary>
        /// The dimensions of the viewport in pixels
        /// </summary>
        protected Point PixelDimensions { get; set; }

        /// <param name="pixelDimensions">
        /// <inheritdoc cref="PixelDimensions" path="/summary"/>
        /// </param>
        public Camera(Point pixelDimensions)
        {
            _zoom = 1;
            Focus = Vector2.Zero;
            PixelDimensions = pixelDimensions;
        }

        /// <summary>
        /// Encapsulates this <see cref="Camera"/> as a <see cref="Matrix"/>
        /// </summary>
        /// <returns>
        /// A <see cref="Matrix"/> that transforms world coordinates to screen coordinates
        /// </returns>
        public Matrix Transform()
        {
            return Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0)
                * Matrix.CreateScale(Zoom, Zoom, 1)
                * Matrix.CreateTranslation(PixelDimensions.X / 2, PixelDimensions.Y / 2, 0);
        }

        private void ClampViewportToBounds()
        {
            if (Bounds.HasValue)
                _focus = new Vector2(
                    Math.Clamp(_focus.X, Bounds.Value.Left + Viewport.Width / 2, Bounds.Value.Right - Viewport.Width / 2), 
                    Math.Clamp(_focus.Y, Bounds.Value.Top + Viewport.Height / 2, Bounds.Value.Bottom - Viewport.Height / 2)
                );
        }
    }
}