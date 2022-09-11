using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class Transform : Component
    {
        public Vector2 Position;
        public float Rotation;
        public float Depth;
        public float Scale;

        public Transform(Vector2 position, float depth, float rotation, float scale)
        {
            Position = position;
            Rotation = rotation;
            Depth = depth;
            Scale = scale;
        }
    }
}