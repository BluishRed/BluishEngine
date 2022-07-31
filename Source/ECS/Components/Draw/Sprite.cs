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
        /// The location of the image
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// The sprite data after it has been loaded
        /// </summary>
        public Texture2D Texture { get; set; }
        /// <summary>
        /// An optional <see cref="Rectangle"/> specifying a particular part of the image
        /// </summary>
        public Rectangle? Source { get; set; }

        /// <param name="location">
        /// The location of the image
        /// </param>
        /// <param name="source">
        /// An optional <see cref="Rectangle"/> specifying a particular part of the image
        /// </param>
        public Sprite(string location, Rectangle? source = null)
        {
            Location = location;            
            Source = source;
        }
    }
}