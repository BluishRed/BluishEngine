using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Systems
{
    public class MovePositions : UpdateSystem
    {
        public MovePositions(World world) : base(world, typeof(Components.Transform), typeof(Components.PositionControllable))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<Components.PositionControllable>().Active)
            {
                Vector2 position = components.GetComponent<Components.Transform>().Position;

                if (Input.IsKeyInState(components.GetComponent<Components.PositionControllable>().Keys[Direction.Up].Item1, components.GetComponent<Components.PositionControllable>().Keys[Direction.Up].Item2))
                {
                    position.Y -= 1;
                }
                if (Input.IsKeyInState(components.GetComponent<Components.PositionControllable>().Keys[Direction.Down].Item1, components.GetComponent<Components.PositionControllable>().Keys[Direction.Down].Item2))
                {
                    position.Y += 1;
                }
                if (Input.IsKeyInState(components.GetComponent<Components.PositionControllable>().Keys[Direction.Left].Item1, components.GetComponent<Components.PositionControllable>().Keys[Direction.Left].Item2))
                {
                    position.X -= 1;
                }
                if (Input.IsKeyInState(components.GetComponent<Components.PositionControllable>().Keys[Direction.Right].Item1, components.GetComponent<Components.PositionControllable>().Keys[Direction.Right].Item2))
                {
                    position.X += 1;
                }

                components.GetComponent<Components.Transform>().Position = position;
            }
        }
    }
}