using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    /// <summary>
    /// Component containing the width and height of the entity
    /// </summary>
    public class Dimensions : Component
    {
        public int Width;
        public int Height;

        public Dimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}