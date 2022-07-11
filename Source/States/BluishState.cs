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
        private Map _map;

        public void AddMap(string location)
        {
            _map = new Map(location);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_map is not null)
                _map.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (_map is not null)
                _map.Update(gameTime);
            base.Update(gameTime);
        }

        public override void LoadContent()
        {
            if (_map is not null)
                _map.LoadContent(Content);
            base.LoadContent();
        }
    }
}