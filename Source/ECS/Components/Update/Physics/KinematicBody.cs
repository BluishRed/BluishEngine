using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    /// <summary>
    /// Component signifying that the entity acts according to Newtonian mechanics, and can have forces applied to it
    /// </summary>
    public class KinematicBody : Component
    {
        /// <summary>
        /// The force vector to apply this frame
        /// </summary>
        public Vector2 Force { get; set; }
        /// <summary>
        /// The veloctity vector that is persistant across frames
        /// </summary>
        public Vector2 Velocity { get; set; }
        /// <summary>
        /// The mass scalar of the entity
        /// </summary>
        public float Mass { get; set; }

        /// <param name="mass">
        /// The mass scalar of the entity
        /// </param>
        public KinematicBody(float mass)
        {
            Mass = mass;
            Force = Vector2.Zero;
        }
    }
}