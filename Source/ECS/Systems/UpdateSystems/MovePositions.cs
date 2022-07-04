using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public class MovePositions : UpdateSystem
    {
        public MovePositions(World world) : base(world, typeof(Transform), typeof(PositionControllable))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, int entity, ComponentCollection components)
        {
            if (components.GetComponent<PositionControllable>().Active)
            {
                // TODO: Add a static input controller
                KeyboardState keyboardState = Keyboard.GetState();

                Vector2 position = components.GetComponent<Transform>().Position;

                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    position.Y -= 1;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    position.Y += 1;
                }
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    position.X -= 1;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    position.X += 1;
                }

                components.GetComponent<Transform>().Position = position;
            }
        }
    }
}