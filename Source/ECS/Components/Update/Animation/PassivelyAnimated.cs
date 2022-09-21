using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Components
{
    public class PassivelyAnimated : Component
    {
        public (Rectangle source, float duration)[] Frames { get; set; }
        public int CurrentFrame { get; set; }
        public float Timer { get; set; }

        public PassivelyAnimated((Rectangle source, float duration)[] frames)
        {
            Frames = frames;
        }
    }
}