using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine
{
    public struct BluishGameParameters
    {
        public Point Dimensions { get; private set; }
        public Type InitialState { get; private set; }

        public BluishGameParameters(Point dimensions, Type initialState)
        {
            Dimensions = dimensions;
            if (initialState.IsSubclassOf(typeof(State)))
                InitialState = initialState;
            else
                throw new Exception($"{initialState} is not a type of State");
        }
    }
}