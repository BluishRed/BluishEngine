using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine
{
    partial class BluishGame
    {
        protected override void Update(GameTime gameTime)
        {
            Input.Update();

            if (Input.IsKeyJustPressed(Keys.Escape))
            {
                Exit();
            }

            if (Input.IsKeyJustPressed(Keys.F11))
            {
                _graphics.ToggleFullScreen();
            }

            StateManager.CurrentState.Update(gameTime);
        }
    }
}