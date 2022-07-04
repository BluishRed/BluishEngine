using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public class PositionControllable : Component
    {
        public bool Active { get; set; }

        public PositionControllable(bool active)
        {
            Active = active;
        }
    }
}