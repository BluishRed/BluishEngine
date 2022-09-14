using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;

namespace BluishEngine.Components
{
    /// <summary>
    /// Component containing a <see cref="Texture2D"/> that is loaded by a <see cref="Systems.SpriteLoader"/>
    /// </summary>
    public class Sprite : Component
    {
        /// <summary>
        /// The content location
        /// </summary>
        public string Location;
        /// <summary>
        /// The sprite data after it has been loaded
        /// </summary>
        public Texture2D Texture;
        /// <summary>
        /// An optional <see cref="Rectangle"/> specifying a particular part of the <see cref="Texture"/>
        /// </summary>
        public Rectangle? Source;

        /// <param name="location">
        /// <inheritdoc cref="Location" path="/summary"/>
        /// </param>
        /// <param name="source">
        /// <inheritdoc cref="Source" path="/summary"/>
        /// </param>
        public Sprite(string location, Rectangle? source = null)
        {
            Location = location;
            Source = source;
        }
    }
}