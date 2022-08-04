using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class KinematicState : Component
    {
        public bool OnGround { get; set; }

        public KinematicState()
        {
            
        }
    }
}