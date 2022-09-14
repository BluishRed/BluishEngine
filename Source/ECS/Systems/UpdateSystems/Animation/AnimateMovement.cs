using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class AnimateMovement : UpdateSystem
    {
        public AnimateMovement(World world) : base(world, typeof(Sprite), typeof(AnimatedMovement), typeof(Collidable), typeof(KinematicBody))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            Rectangle sprite;
            Direction xDirection;

            if (components.GetComponent<KinematicBody>().Velocity.X != 0)
            {
                xDirection = components.GetComponent<KinematicBody>().Velocity.X < 0 ? Direction.Left : Direction.Right;
            }
            else
            {
                xDirection = components.GetComponent<AnimatedMovement>().lastDirection;
            }

            if (!components.GetComponent<Collidable>().OnGround)
            {
                if (components.GetComponent<KinematicBody>().Velocity.Y < 0)
                {
                    sprite = xDirection == Direction.Left ? components.GetComponent<AnimatedMovement>().JumpFrames.facingLeft : components.GetComponent<AnimatedMovement>().JumpFrames.facingRight;
                }
                else
                {
                    sprite = xDirection == Direction.Left ? components.GetComponent<AnimatedMovement>().FallFrames.facingLeft : components.GetComponent<AnimatedMovement>().FallFrames.facingRight;
                }
            }
            else
            {
                if (components.GetComponent<KinematicBody>().Velocity.X != 0)
                {
                    components.GetComponent<AnimatedMovement>().WalkFrameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (components.GetComponent<AnimatedMovement>().WalkFrameTimer >= components.GetComponent<AnimatedMovement>().WalkFrameTime)
                    {
                        components.GetComponent<AnimatedMovement>().WalkFrameTimer %= components.GetComponent<AnimatedMovement>().WalkFrameTime;
                        components.GetComponent<AnimatedMovement>().WalkIndex++;
                        components.GetComponent<AnimatedMovement>().WalkIndex %= components.GetComponent<AnimatedMovement>().WalkFrames.facingLeft.Length;
                    }

                    sprite = xDirection == Direction.Left ? components.GetComponent<AnimatedMovement>().WalkFrames.facingLeft[components.GetComponent<AnimatedMovement>().WalkIndex] : components.GetComponent<AnimatedMovement>().WalkFrames.facingRight[components.GetComponent<AnimatedMovement>().WalkIndex];
                }
                else
                {
                    sprite = xDirection == Direction.Left ? components.GetComponent<AnimatedMovement>().IdleFrames.facingLeft : components.GetComponent<AnimatedMovement>().IdleFrames.facingRight;
                    components.GetComponent<AnimatedMovement>().WalkIndex = 0;
                }
            }

            components.GetComponent<AnimatedMovement>().lastDirection = xDirection;
            components.GetComponent<Sprite>().Source = sprite;
        }
    }
}