using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    public class CanJump : Component
    {
        public float Force { get; set; }
        public (Keys, KeyPressState) Controls { get; set; }

        public CanJump(
            (Keys, KeyPressState) controls,
            float force)
        {
            Force = force;
            Controls = controls;
        }
    }
}