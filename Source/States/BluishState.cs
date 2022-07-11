using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public abstract class BluishState : State
    {
        public void AddMap(string location)
        {
            SubWorlds.Add(new Map(location));
        }
    }
}