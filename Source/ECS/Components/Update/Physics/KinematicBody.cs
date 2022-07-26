using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class KinematicBody : Component
    {
        public Vector2 Force { get; set; }
        public Vector2 Velocity { get; set; }
        public float Mass { get; set; }

        public KinematicBody(float mass)
        {
            Mass = mass;
            Force = Vector2.Zero;
        }
    }
}