using System;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BluishFramework;
using BluishEngine.Components;

namespace BluishEngine.Systems
{
    public class Lights : UpdateSystem
    {
        private List<PointLight> _lightList;

        public Lights(World world, List<PointLight> lights) : base(world, typeof(Transform), typeof(Dimensions), typeof(LightSource))
        {
            _lightList = lights;
        }

        protected override void UpdateEntity(GameTime gameTime, int entity, ComponentCollection components)
        {
            _lightList.Add(new PointLight(new Vector2(components.GetComponent<Transform>().Position.X + components.GetComponent<Dimensions>().Width / 2, components.GetComponent<Transform>().Position.Y + components.GetComponent<Dimensions>().Height / 2), components.GetComponent<Transform>().Depth, components.GetComponent<LightSource>().Radius, components.GetComponent<LightSource>().Brightness));
        }
    }
}