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
        private Map _map;
        private Rectangle _previousRoom;

        public CameraFollowEntity(World world, Camera camera, Map map = null) : base(world, typeof(CameraFollowable), typeof(Transform), typeof(Dimensions))
        {
            _camera = camera;
            _map = map;
            _previousRoom = new Rectangle(0, 0, 320, 180);
        }
        
        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<CameraFollowable>().Active)
            {
                Vector2 centre = new Vector2(components.GetComponent<Transform>().Position.X + components.GetComponent<Dimensions>().Width / 2, components.GetComponent<Transform>().Position.Y + components.GetComponent<Dimensions>().Height / 2);

                if (_map is not null)
                {
                    Rectangle newRoom = _map.GetRoomContainingVector(centre);                    
                  
                    if (newRoom != _previousRoom)
                    {
                        _camera.Bounds = Rectangle.Union(_previousRoom, newRoom);
                        _camera.SlideTo(newRoom.Location.ToVector2(), 1);
                    }
                    else
                    {
                        _camera.Bounds = newRoom;
                        _camera.FocusOn(centre);
                    }

                    _previousRoom = newRoom;
                }
            }
        }
    }
}