using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class CanWalk : Component
    {
        public (Keys, KeyPressState) LeftControls { get; set; }
        public (Keys, KeyPressState) RightControls { get; set; }
        public float Force { get; set; }

        public CanWalk(
            (Keys, KeyPressState) leftControls,
            (Keys, KeyPressState) rightControls,
            float force
        )
        {
            LeftControls = leftControls;
            RightControls = rightControls;
            Force = force;
        }
    }
}