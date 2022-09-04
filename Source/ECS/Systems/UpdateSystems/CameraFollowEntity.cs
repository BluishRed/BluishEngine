using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;
using BluishEngine.Components;
using Microsoft.Xna.Framework.Media;

namespace BluishEngine.Systems
{
    public class CameraFollowEntity : UpdateSystem
    {
        private Camera _camera;
        private Map _map;
        private Rectangle _previousRoom;

        public CameraFollowEntity(World world, Camera camera, Map map = null) : base(world, typeof(CameraFollowable), typeof(Transform), typeof(KinematicBody), typeof(Dimensions))
        {
            _camera = camera;
            _map = map;
            _previousRoom = new Rectangle(0, 0, 320, 180);
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            // TODO: Lock entity in place while room transitions

            if (components.GetComponent<CameraFollowable>().Active)
            {          
                Vector2 centre = new Vector2(components.GetComponent<Transform>().Position.X + components.GetComponent<Dimensions>().Width / 2f, components.GetComponent<Transform>().Position.Y + components.GetComponent<Dimensions>().Height / 2f);
                _camera.SmoothFocusOn(gameTime, centre, 0.3f, components.GetComponent<KinematicBody>().Velocity);

                if (_map is not null)
                {
                    Rectangle newRoom = _map.GetRoomContainingVector(centre);

                    if (newRoom != _previousRoom)
                    {
                        _camera.Bounds = Rectangle.Union(_previousRoom, newRoom);
                        _camera.SlideTo(newRoom.Location.ToVector2(), 1f);
                    }
                    else
                    {
                        _camera.Bounds = newRoom;
                    }

                    _previousRoom = newRoom;
                }

                if (Input.IsKeyJustPressed(Keys.W))
                {
                    _camera.ZoomBy(2, 0.5f);
                }
                if (Input.IsKeyJustPressed(Keys.S))
                {
                    _camera.ZoomBy(0.5f, 0.5f);
                }
            }
        }
    }
}