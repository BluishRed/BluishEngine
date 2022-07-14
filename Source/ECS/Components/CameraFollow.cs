using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    public class CameraFollow : Component
    {
        public bool Active { get; set; }

        public CameraFollow(bool active)
        {
            Active = active;
        }
    }
}