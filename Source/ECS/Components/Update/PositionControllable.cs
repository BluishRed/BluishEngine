using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    public class PositionControllable : Component
    {
        public bool Active { get; set; }
        public Dictionary<Direction, (Keys, KeyPressState)> Keys { get; set; }

        public PositionControllable(bool active, params (Direction, Keys, KeyPressState)[] keys)
        {
            Active = active;
            Keys = new Dictionary<Direction, (Keys, KeyPressState)>();
            foreach ((Direction, Keys, KeyPressState) key in keys)
            {
                Keys.Add(key.Item1, (key.Item2, key.Item3));
            }
        }
    }
}

namespace BluishEngine
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}