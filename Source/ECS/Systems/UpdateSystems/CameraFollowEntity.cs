using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using System.Diagnostics;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class CameraFollowEntity : UpdateSystem
    {
        private Camera _camera;
        private Map? _map;
        private bool _canEntityMove;
        private Rectangle _previousRoom;

        public CameraFollowEntity(World world, Camera camera, Map? map = null) : base(world, typeof(CameraFollowable), typeof(Transform), typeof(KinematicBody), typeof(Dimensions))
        {
            _camera = camera;
            _map = map;
            _previousRoom = new Rectangle(0, 0, 320, 180);
            _canEntityMove = true;
        }

        protected override void UpdateEntity(GameTime gameTime, Entity entity, ComponentCollection components)
        {
            if (components.GetComponent<CameraFollowable>().Active)
            {
                Vector2 centre = new Vector2(components.GetComponent<Transform>().Position.X + components.GetComponent<Dimensions>().Width / 2, components.GetComponent<Transform>().Position.Y + components.GetComponent<Dimensions>().Height / 2);
                _camera.FocusOn(centre);

                if (_map is not null)
                {
                    Rectangle currentRoom = _map.GetRoomContainingVector(centre);

                    if (currentRoom != _previousRoom)
                    {
                        _camera.Bounds = Rectangle.Union(_previousRoom, currentRoom);
                        _camera.SlideTo(GetRoomTarget(currentRoom), 0.6f, UnlockPlayer);
                        _canEntityMove = false;
                    }
                    else
                    {
                        _camera.Bounds = currentRoom;
                    }

                    _previousRoom = currentRoom;
                }

                if (Input.IsKeyJustPressed(Keys.W))
                {
                    _camera.ZoomBy(2, 0.5f);
                }
                if (Input.IsKeyJustPressed(Keys.S))
                {
                    _camera.ZoomBy(0.5f, 0.5f);
                }

                components.GetComponent<KinematicBody>().CanMove = _canEntityMove;
            }
        }

        private Vector2 GetRoomTarget(Rectangle room)
        {
            Vector2 target = room.Location.ToVector2();

            if (room.X > _previousRoom.X)
            {
                target.X = _camera.Position.X + _camera.Viewport.Width;
            }
            else if (room.X < _previousRoom.X)
            {
                target.X = _camera.Position.X - _camera.Viewport.Width;
            }
            else
            {
                target.X = _camera.Position.X;
            }

            if (room.Y > _previousRoom.Y)
            {
                target.Y = _camera.Position.Y + _camera.Viewport.Height;
            }
            else if (room.Y < _previousRoom.Y)
            {
                target.Y = _camera.Position.Y - _camera.Viewport.Height;
            }
            else
            {
                target.Y = _camera.Position.Y;
            }

            return target;
        }

        private void UnlockPlayer()
        {
            _canEntityMove = true;
        }
    }
}