using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class CameraFollowEntity : UpdateSystem
    {
        private Camera _camera;
        private Map? _map;

        public CameraFollowEntity(World world, Camera camera, Map? map = null) : base(world, typeof(CameraFollowable), typeof(Transform), typeof(Dimensions))
        {
            _camera = camera;
            _map = map;
        }
        
        protected override void UpdateEntity(GameTime gameTime, int entity, ComponentCollection components)
        {
            if (components.GetComponent<CameraFollowable>().Active)
            {
                Vector2 centre = new Vector2(components.GetComponent<Transform>().Position.X + components.GetComponent<Dimensions>().Width / 2, components.GetComponent<Transform>().Position.Y + components.GetComponent<Dimensions>().Height / 2);
                
                if (_map is not null)
                    _camera.Bounds = _map.GetRoomContainingVector(centre);

                _camera.Focus = centre;
            }
        }
    }
}