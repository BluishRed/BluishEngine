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
        public ControlEntity(World world) : base(world, typeof(Transform), typeof(KinematicBody), typeof(PositionControllable))
        {
        }

        // TODO: Collision not working
        // TODO: Camera stabilisation

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<PositionControllable>().Active)
            {
                Vector2 force = components.GetComponent<KinematicBody>().Force;

                if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Up)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Up].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Up].Item2))
                {
                    force.Y -= 1f;
                }
                else if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Down)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Down].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Down].Item2))
                {
                    force.Y += 1f;
                }

                if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Left)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Left].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Left].Item2))
                {
                    force.X -= 1f;
                }
                else if (components.GetComponent<PositionControllable>().Keys.ContainsKey(Direction.Right)
                    && Input.IsKeyInState(components.GetComponent<PositionControllable>().Keys[Direction.Right].Item1, components.GetComponent<PositionControllable>().Keys[Direction.Right].Item2))
                {
                    force.X += 1f;
                }

                if (Input.IsKeyJustPressed(Keys.Space))
                {
                    force.Y -= 340;
                }

                components.GetComponent<KinematicBody>().Force = force;
            }
        }
    }
}