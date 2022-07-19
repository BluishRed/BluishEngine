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
        public Vector2 Focus { get; set; }
        public float Zoom { get; set; }
        protected Point Dimensions { get; set; }

        public Camera(Point screenDimensions)
        {
            Zoom = 1;
            Focus = Vector2.Zero;
            Dimensions = screenDimensions;
        }

        public Matrix Transform()
        {
            return Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0)
                * Matrix.CreateScale(Zoom)
                * Matrix.CreateTranslation(Dimensions.X / 2, Dimensions.Y / 2, 0);
        }

        public Rectangle GetViewport()
        {
            Matrix invMatrix = Matrix.Invert(Transform());

            Point TL = Vector2.Transform(new Vector2(0, 0), invMatrix).ToPoint();
            Point BR = Vector2.Transform(new Vector2(Dimensions.X, Dimensions.Y), invMatrix).ToPoint();

            return new Rectangle(TL.X, TL.Y, BR.X - TL.X, BR.Y - TL.Y);
        }
    }
}