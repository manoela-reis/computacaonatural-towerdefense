using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame5
{
    class Animacao
    {

        public Texture2D texture;
        public Texture2D[] textures = new Texture2D[2];
        private int id;
        private float cronometro;
        private float tempo;

        public Animacao(float tempo, int qntTexturas)
        {
            this.tempo = tempo;
            textures = new Texture2D[qntTexturas];
        }

        public void Update(GameTime e){
            cronometro += (float)e.ElapsedGameTime.Milliseconds / 1000;
            
            if(cronometro >= tempo)
            {
                cronometro = 0;                
                id++;
                if (id > textures.Length - 1)
                {
                    id = 0;
                }
                texture = textures[id];
            }
        }
    }
}
