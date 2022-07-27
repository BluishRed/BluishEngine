using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;


namespace BluishEngine
{
    public class Camera
    {
        private Point _focus;
        public Point Focus 
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
        public float Zoom { get; set; }
        public Rectangle Viewport
        {
            get
            {
                int width = (int)Math.Ceiling(Dimensions.X / Zoom);
                int height = (int)Math.Ceiling(Dimensions.Y / Zoom);
                return new Rectangle(Focus.X - (int)Math.Ceiling(width / 2f), Focus.Y - (int)Math.Ceiling(height / 2f), width, height);
            }
            set
            {
                Focus = value.Center;
                Zoom = (float)Dimensions.Y / value.Height;
                ClampViewportToBounds();
            }
        }
        public Rectangle? Bounds { get; set; }
        protected Point Dimensions { get; set; }

        public Camera(Point screenDimensions)
        {
            Zoom = 1;
            Focus = Point.Zero;
            Dimensions = screenDimensions;
        }

        public Matrix Transform()
        {
            return Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0)
                * Matrix.CreateScale(Zoom)
                * Matrix.CreateTranslation(Dimensions.X / 2, Dimensions.Y / 2, 0);
        }
        
        private void ClampViewportToBounds()
        {
            if (Bounds is not null)
                _focus = new Rectangle(Math.Clamp(Viewport.X, Bounds.Value.Left, Bounds.Value.Right - Viewport.Width), Math.Clamp(Viewport.Y, Bounds.Value.Top, Bounds.Value.Bottom - Viewport.Height), Viewport.Width, Viewport.Height).Center;
        }
    }
}