﻿using System;
using System.Resources;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BluishFramework;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using System.Globalization;

namespace BluishEngine
{
    static class Effects
    {
        private static Dictionary<string, Effect> _effects;

        static Effects()
        {
            _effects = new Dictionary<string, Effect>();
        }

        public static void LoadAssets(ContentManager content)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            ResourceManager resourceManager = new ResourceManager("BluishEngine.Resources", assembly);
            content = new ResourceContentManager(content.ServiceProvider, resourceManager);
            ResourceSet resourceSet = resourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true);

            foreach (DictionaryEntry entry in resourceSet)
            {
                _effects.Add(entry.Key.ToString(), content.Load<Effect>(entry.Key.ToString()));
            }
        }

        public static Effect GetEffect(string name)
        {
            return _effects[name];
        }
    }
}