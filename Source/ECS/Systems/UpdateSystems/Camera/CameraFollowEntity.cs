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
            Vector2 centre = new Vector2(components.GetComponent<Transform>().Position.X + components.GetComponent<Dimensions>().Width / 2, components.GetComponent<Transform>().Position.Y + components.GetComponent<Dimensions>().Height / 2);
            _camera.SmoothFocusOn(gameTime, centre, 0.6f);

            if (_map is not null)
            {
                Rectangle currentRoom = _map.GetRoomContainingVector(centre);

                if (currentRoom != _previousRoom)
                {
                    _camera.Bounds = Rectangle.Union(_previousRoom, currentRoom);

                    if (currentRoom.Y < _previousRoom.Y)
                    {
                        components.GetComponent<KinematicBody>().Force.Y -= 150;
                    }

                    _camera.SlideTo(GetRoomTarget(centre), 0.6f, UnlockPlayer);
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

        private Vector2 GetRoomTarget(Vector2 centre)
        {
            Camera camera = new Camera(_camera.Viewport.Size.ToPoint());
            camera.Bounds = _map.GetRoomContainingVector(centre);
            camera.FocusOn(centre);
            return Vector2.Floor(camera.Position);
        }

        private void UnlockPlayer()
        {
            _canEntityMove = true;
        }
    }
}