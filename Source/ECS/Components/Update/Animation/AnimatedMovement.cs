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
        public Rectangle TurnFrame;
        public (Rectangle facingLeft, Rectangle facingRight) JumpFrames;
        public (Rectangle facingLeft, Rectangle facingRight) FallFrames;
        public (Rectangle facingLeft, Rectangle facingRight) IdleFrames;
        public (Rectangle[] facingLeft, Rectangle[] facingRight) WalkFrames;
        public float WalkFrameTime;
        public float WalkFrameTimer;
        public int WalkIndex;
        public float TurnTimer;
        public Direction lastDirection;

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
        }
    }
}