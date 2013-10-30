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
    public class KeyboardManager
    {
        public KeyboardManager()
        {
        }

        public KeyboardState _Keyboard { get; set; }
        public KeyboardState _OldKeyboard { get; set; }

        public void Update()
        {
            _Keyboard = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys k)
        {
            return this._Keyboard.IsKeyDown(k) &&
                   this._OldKeyboard.IsKeyUp(k);
        }

        public bool IsKeyUp(Keys k)
        {
            return this._Keyboard.IsKeyUp(k) &&
                   this._OldKeyboard.IsKeyDown(k);
        }

        public void LateUpdate()
        {
            _OldKeyboard = _Keyboard;
        }
    }
}
