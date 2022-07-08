using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class Sprite : Component
    {
        public string Location { get; set; }
        public Texture2D Texture { get; set; }
        public Rectangle? Source { get; set; }

        public Sprite(string location, Rectangle? source = null)
        {
            Location = location;            
            Source = source;
        }
    }
}