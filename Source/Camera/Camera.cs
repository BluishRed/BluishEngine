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
        public Point Focus { get; set; }
        public float Zoom { get; set; }
        public Rectangle Viewport
        {
            get
            {
                int width = (int)Math.Ceiling(Dimensions.X / Zoom) + 1;
                int height = (int)Math.Ceiling(Dimensions.Y / Zoom) + 1;
                return new Rectangle(Focus.X - (int)Math.Ceiling(width / 2f), Focus.Y - (int)Math.Ceiling(height / 2f), width, height);
            }
            set
            {
                Focus = value.Center;
                Zoom = (float)Dimensions.Y / value.Height;
            }
        }
        protected Point Dimensions { get; set; }
        protected Rectangle Bounds { get; set; }

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
        
        public void ClampViewport(int minX, int maxX, int minY, int maxY)
        {
            Focus = new Rectangle(Math.Clamp(Viewport.X, minX, maxX - Viewport.Width), Math.Clamp(Viewport.Y, minY, maxY - Viewport.Height), Viewport.Width, Viewport.Height).Center;
        }
    }
}