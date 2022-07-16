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
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        protected Dimensions Dimensions { get; set; }

        public Camera(Dimensions screenDimensions)
        {
            Zoom = 1;
            Rotation = 0;
            Focus = Vector2.Zero;
            Dimensions = screenDimensions;
        }

        public Matrix Transform()
        {
            return Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0)
                * Matrix.CreateRotationZ(Rotation)
                * Matrix.CreateScale(Zoom)
                * Matrix.CreateTranslation(Dimensions.Width / 2, Dimensions.Height / 2, 0);
        }
    }
}