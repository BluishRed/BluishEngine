using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishEngine.Input;

namespace BluishEngine
{
    partial class BluishGame
    {
        protected override void Update(GameTime gameTime)
        {
            InputState.Update();
            ExecuteUniversalKeys();
            StateManager.CurrentState.Update(gameTime);
        }

        private void ExecuteUniversalKeys()
        {
            InputHandler.ExecuteInputs(_commonInputMap);
        }
    }
}