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
        private Vector2 _focus;
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
        public float Zoom { get; set; }
        public Rectangle Viewport
        {
            get
            {
                //Matrix inverse = Matrix.Invert(Transform());
                //Point TL = Vector2.Transform(Vector2.Zero, inverse).ToPoint();
                //Point BR = Vector2.Transform(Dimensions.ToVector2(), inverse).ToPoint();

                //return new Rectangle(TL, BR - TL);
                int width = (int)Math.Ceiling(Dimensions.X / Zoom);
                int height = (int)Math.Ceiling(Dimensions.Y / Zoom);
                return new Rectangle((int)(Focus.X - Math.Ceiling(width / 2f)), (int)(Focus.Y - (int)Math.Ceiling(height / 2f)), width, height);
            }
            set
            {
                Focus = value.Center.ToVector2();
                Zoom = (float)Dimensions.Y / value.Height;
                ClampViewportToBounds();
            }
        }
        public Rectangle? Bounds { get; set; }
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
        
        private void ClampViewportToBounds()
        {
            if (Bounds is not null)
                _focus = new Vector2(
                    Math.Clamp(_focus.X, Bounds.Value.Left + Viewport.Width / 2, Bounds.Value.Right - Viewport.Width / 2), 
                    Math.Clamp(_focus.Y, Bounds.Value.Top + Viewport.Height / 2, Bounds.Value.Bottom - Viewport.Height / 2)
                );
        }
    }
}