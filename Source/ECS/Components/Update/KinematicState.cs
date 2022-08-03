using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class KinematicState : Component
    {
        public enum PositionState
        { 
            Ground,
            Air
        }

        public enum MovementState
        {
            Stopped,
            Walking,
            Running,
            Jumping
        }

        public PositionState Position { get; set; }
        public MovementState Movement { get; set; }

        public KinematicState()
        {
            Position = PositionState.Ground;
            Movement = MovementState.Stopped;
        }
    }
}