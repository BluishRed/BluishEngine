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
    public class MovePositions : UpdateSystem
    {
        public MovePositions(World world) : base(world, typeof(Transform), typeof(KinematicBody), typeof(PositionControllable))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<PositionControllable>().Active)
            {
                Vector2 velocity = components.GetComponent<KinematicBody>().Velocity;

                if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Up) 
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Up].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Up].Item2))
                {
                    velocity.Y = -1f;
                }
                else if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Down) 
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Down].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Down].Item2))
                {
                    velocity.Y = 1f;
                }
                else
                {
                    velocity.Y = 0f;
                }

                if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Left) 
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Left].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Left].Item2))
                {
                    velocity.X = -1f;
                }
                else if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Right) 
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Right].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Right].Item2))
                {
                    velocity.X = 1f;
                }
                else
                {
                    velocity.X = 0f;
                }

                components.GetComponent<KinematicBody>().Velocity = velocity;
            } 
        }
    }
}