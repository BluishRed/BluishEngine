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
    /// Component signifying that the entity can collide with the map and other collidable entities
    /// </summary>
    public class Collidable : Component
    {
        /// <summary>
        /// The <see cref="Rectangle"/> that defines the bounding box of this entity. The coordinates of the rectangle are relative to the top left of the entity
        /// </summary>
        public Rectangle BoundingBox { get; set; }
        
        /// <param name="boundingBox">
        /// The bounding box relative to the top left of the entity
        /// </param>
        public Collidable(Rectangle boundingBox)
        {
            BoundingBox = boundingBox;
        }
    }
}