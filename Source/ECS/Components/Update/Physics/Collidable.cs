using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;

namespace BluishEngine.Components
{
    /// <summary>
    /// Component signifying that the entity can collide with the map and other entities
    /// </summary>
    public class Collidable : Component
    {
        /// <summary>
        /// 
        /// </summary>
        public Rectangle BoundingBox { get; set; }
        
        /// <param name="boundingBox">
        /// 
        /// </param>
        public Collidable(Rectangle boundingBox)
        {
            BoundingBox = boundingBox;
        }
    }
}