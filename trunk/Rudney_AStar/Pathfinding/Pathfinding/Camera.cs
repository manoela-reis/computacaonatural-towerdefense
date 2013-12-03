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

namespace Pathfinding
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;

        GraphicsDevice device;

        private Matrix Orientation;

        private float[] hAngle = new float[2];
        private float[] vAngle = new float[2];

        public Vector3[] positions = new Vector3[2];

       

        private const float moveAcc = 0.1f;
        private const float lookAcc = 0.2f;
        private const float moveStep = 0.3f;
        private const float lookStep = 0.005f;
        private int yPos;
        private int xPos;
        public bool boleanaaa = false;
        

        public Camera(Game game, Vector3 position)
            : this(game, position, Vector3.Zero)
        {
            
        }

        public Camera(Game game, Vector3 position, Vector3 target)
            : this(game, position, target, Vector3.Up)
        {
            //UpdateOrientation();
            this.View = Matrix.CreateLookAt(position, target + Orientation.Forward, Orientation.Up);
           
           
        }

        public Camera(Game game, Vector3 position, Vector3 target, Vector3 up)
        {
            device = game.GraphicsDevice;
            this.positions[0] = this.positions[1] = position;
            

            var ratio = (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height;
            View = Matrix.CreateLookAt(position, target, up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, ratio, 1, 100);
        }

        public void Update()
        {
            UpdateFromMouse();
            UpdateOrientation();
            UpdateView();
        }

        private void UpdateFromMouse()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //boleanaaa = true;
            }
        }

        

        private void UpdateOrientation()
        {
            Orientation = Matrix.CreateRotationY(hAngle[0]);
            Orientation *= Matrix.CreateFromAxisAngle(Orientation.Left, vAngle[0]);
        }
        
        private void UpdateView()
        {
            this.View = Matrix.CreateLookAt(this.positions[0], this.positions[0] + Orientation.Forward, Orientation.Up);
        }


    }
}
