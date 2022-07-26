using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    public class CameraFollowable : Component
    {
        public bool Active { get; set; }

        public CameraFollowable(bool active)
        {
            Active = active;
        }
    }
}