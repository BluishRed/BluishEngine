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
                int width = (int)(Dimensions.X / Zoom);
                int height = (int)(Dimensions.Y / Zoom);
                return new Rectangle(Focus.X - width / 2, Focus.Y - height / 2, width, height);
            }
            set
            {
                Focus = value.Center;
                Zoom = (float)Dimensions.Y / value.Height;
            }
        }
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

        public void ClampViewport(int minX, int maxX, int minY, int maxY)
        {
            Viewport = new Rectangle(Math.Clamp(Viewport.X, minX, maxX - Viewport.Width), Math.Clamp(Viewport.Y, minY, maxY - Viewport.Height), Viewport.Width, Viewport.Height);
        }
    }
}