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
        public Vector2 Force;
        /// <summary>
        /// The velocity vector that is persistant across frames
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// The change in velocity for this frame
        /// </summary>
        public Vector2 Acceleration;
        /// <summary>
        /// The mass scalar of the entity
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// Flag indicating whether the entity can move
        /// </summary>
        public bool CanMove { get; set; }

        /// <param name="mass">
        /// <inheritdoc cref="Mass" path="/summary"/>
        /// </param>
        public KinematicBody(float mass)
        {
            Mass = mass;
            Force = Vector2.Zero;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            CanMove = true;
        }
    }
}