using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Systems
{
    public class MoveCamera : UpdateSystem
    {
        private Camera _camera;

        public MoveCamera(World world, Camera camera) : base(world, typeof(Components.CameraFollow), typeof(Components.Transform))
        {
            _camera = camera;
        }

        protected override void UpdateEntity(GameTime gameTime, int entity, ComponentCollection components)
        {
            if (components.GetComponent<Components.CameraFollow>().Active)
            {
                _camera.Focus = components.GetComponent<Components.Transform>().Position;
            }
        }
    }
}