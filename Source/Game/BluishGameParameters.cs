using System;
using BluishFramework;

namespace BluishEngine
{
    public struct BluishGameParameters
    {
        public Dimensions Dimensions { get; private set; }
        public Type InitialState { get; private set; }

        public BluishGameParameters(Dimensions dimensions, Type initialState)
        {
            Dimensions = dimensions;
            if (initialState.IsSubclassOf(typeof(State)))
                InitialState = initialState;
            else
                throw new Exception($"{initialState} is not a type of State");
        }
    }
}