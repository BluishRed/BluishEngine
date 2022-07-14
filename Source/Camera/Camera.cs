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

        private Dimensions _screen;

        public Camera()
        {
            Zoom = 1;
            Rotation = 0;
            Focus = Vector2.Zero;
            _screen = Graphics.GameResolution;
        }

        public Matrix Transform()
        {
            return Matrix.CreateTranslation(-Focus.X, -Focus.Y, 0)
                * Matrix.CreateRotationZ(Rotation)
                * Matrix.CreateScale(Zoom)
                * Matrix.CreateTranslation(_screen.Width / 2, _screen.Height / 2, 0);
        }
    }
}