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
        /// <summary>
        /// Optional <see cref="HashSet{T}"/> containing directions that the entity cannot be collided from
        /// </summary>
        public HashSet<Direction> ExcludedDirections { get; set; }

        /// <param name="boundingBox">
        /// <inheritdoc cref="BoundingBox" path="/summary"/>
        /// </param>
        /// <param name="excludedDirections">
        /// <inheritdoc cref="ExcludedDirections" path="/summary"/>
        /// </param>
        public Collidable(Rectangle boundingBox, HashSet<Direction> excludedDirections = null)
        {
            BoundingBox = boundingBox;

            if (excludedDirections is null)
            {
                ExcludedDirections = new HashSet<Direction>();
            }
            else
            {
                ExcludedDirections = excludedDirections;
            }
        }
    }
}