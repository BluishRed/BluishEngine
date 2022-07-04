using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public class Transform : Component
    {
        public Vector2 Position { get; set; }
        public float Depth { get; set; }

        public Transform(Vector2 position, float depth)
        {
            Position = position;
            Depth = depth;
        }
    }
}