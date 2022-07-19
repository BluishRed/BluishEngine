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
        private Map _map;

        public MoveCamera(World world, Camera camera, Map map = null) : base(world, typeof(Components.CameraFollowable), typeof(Components.Transform), typeof(Components.Dimensions))
        {
            _camera = camera;
            _map = map;
        }
        
        protected override void UpdateEntity(GameTime gameTime, int entity, ComponentCollection components)
        {
            if (components.GetComponent<Components.CameraFollowable>().Active)
            {
                _camera.Focus = new Vector2(components.GetComponent<Components.Transform>().Position.X + components.GetComponent<Components.Dimensions>().Width / 2, components.GetComponent<Components.Transform>().Position.Y + components.GetComponent<Components.Dimensions>().Height / 2);
            }

            if (_map is not null)
            {
                Rectangle viewport = _camera.GetViewport();

                _camera.Focus = new Vector2(Graphics.GameResolution.X / 2 + Math.Clamp(viewport.X, 0, _map.Dimensions.X), Graphics.GameResolution.Y / 2 + Math.Clamp(viewport.Y, 0, _map.Dimensions.Y));
            }
        }
    }
}