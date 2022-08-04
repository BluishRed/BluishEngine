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
    public class ControlEntity : UpdateSystem
    {

        const float _speed = 11f;
        const float _jump = 320f;

        public ControlEntity(World world) : base(world, typeof(Transform), typeof(KinematicBody), typeof(PositionControllable), typeof(KinematicState))
        {
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<PositionControllable>().Active)
            {
                Vector2 force = components.GetComponent<KinematicBody>().Force;

                if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Up)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Up].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Up].Item2))
                {
                    force.Y -= _speed;
                }
                else if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Down)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Down].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Down].Item2))
                {
                    force.Y += _speed;
                }

                if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Left)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Left].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Left].Item2))
                {
                    force.X -= _speed;
                }
                else if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Right)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Right].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Right].Item2))
                {
                    force.X += _speed;
                }

                if (Input.IsKeyJustPressed(Keys.Space) && components.GetComponent<KinematicState>().OnGround)
                {
                    force.Y -= _jump;
                }

                components.GetComponent<KinematicBody>().Force = force;
            }
        }
    }
}