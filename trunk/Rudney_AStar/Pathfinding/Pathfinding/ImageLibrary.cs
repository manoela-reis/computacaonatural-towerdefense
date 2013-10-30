using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pathfinding
{
    public class ImageLibrary
    {
        private Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

        private static ImageLibrary _instance;

        private ImageLibrary() { }

        public static ImageLibrary getInstance()
        {
            if(_instance == null)
                _instance = new ImageLibrary();

            return _instance;
        }

        public void putImage(string key,Texture2D image)
        {
            images.Add(key,image);
        }

        public Texture2D getImage(string key)
        {
            if(images.ContainsKey(key))
                return images[key];

            return null;
        }

    }
}
