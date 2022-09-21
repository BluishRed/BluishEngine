using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    public class AnimatedMovement : Component
    {
        // TODO: Accomodate for multiple jumping/falling frames
        public Rectangle TurnFrame { get; }
        public (Rectangle facingLeft, Rectangle facingRight) JumpFrames { get; }
        public (Rectangle facingLeft, Rectangle facingRight) FallFrames { get; }
        public (Rectangle facingLeft, Rectangle facingRight) IdleFrames { get; }
        public (Rectangle[] facingLeft, Rectangle[] facingRight) WalkFrames { get; }
        public float WalkFrameTime { get; }
        public float WalkFrameTimer { get; set; }
        public int WalkIndex { get; set; }
        public float TurnTimer { get; set; }
        public Direction lastDirection { get; set; }

        public AnimatedMovement(
            Rectangle turnFrame,
            (Rectangle facingLeft, Rectangle facingRight) jumpFrames,
            (Rectangle facingLeft, Rectangle facingRight) fallFrames,
            (Rectangle facingLeft, Rectangle facingRight) idleFrames,
            (Rectangle[] facingLeft, Rectangle[] facingRight) walkFrames,
            float secondsBetweenWalkFrames
        )
        {
            TurnFrame = turnFrame;
            JumpFrames = jumpFrames;
            FallFrames = fallFrames;
            IdleFrames = idleFrames;
            WalkFrames = walkFrames;
            WalkFrameTime = secondsBetweenWalkFrames;
            lastDirection = Direction.Right;
        }
    }
}