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
    /// Component signifying that the entity can be tracked by the camera
    /// </summary>
    public class CameraFollowable : Component
    {
        /// <summary>
        /// Boolean signifying whether the camera should follow this entity
        /// </summary>
        public bool Active;

        /// <param name="active">
        /// <inheritdoc cref="Active" path="/summary"/>
        /// </param>
        public CameraFollowable(bool active)
        {
            Active = active;
        }
    }
}