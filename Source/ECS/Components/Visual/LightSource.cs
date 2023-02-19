using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class LightSource : Component
    {
        public int Radius { get; set; }
        public float Brightness { get; set; }

        // TODO: Choose how bright the light is
        public LightSource(int radius, float brightness)
        {
            Radius = radius;
            Brightness = brightness;
        }
    }
}