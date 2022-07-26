using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    public class Collidable : Component
    {
        public Rectangle BoundingBox { get; set; }

        public Collidable(Rectangle boundingBox)
        {
            BoundingBox = boundingBox;
        }
    }
}