using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine
{
    public class PointLight
    {
        public Vector2 Position { get; private set; }
        public float Depth { get; private set; }
        public int Radius { get; private set; }
        public float Brightness { get; private set; }

        public PointLight(Vector2 position, float depth, int radius, float brightness)
        {
            Position = position;
            Radius = radius;
            Depth = depth;
            Brightness = brightness;
        }
    }
}
