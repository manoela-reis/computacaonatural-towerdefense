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
    class Arvore
    {
        Texture2D textureMoinho;
        public VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;
        public Matrix world;
        public Vector3 position;

        public Arvore(Vector3 position, Vector3 positionCamera, GraphicsDevice graphicsDevice)
        {
            world = Matrix.CreateConstrainedBillboard(position, positionCamera, Vector3.Up, Vector3.Forward, Vector3.Forward);

            this.position = position;

            verts = new VertexPositionTexture[4];
            verts[0] = new VertexPositionTexture(new Vector3(-1, 3, 0), new Vector2(0, 0));
            verts[1] = new VertexPositionTexture(new Vector3(1, 3, 0), new Vector2(1, 0));
            verts[2] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            verts[3] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture),
                verts.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(verts);
        }

        public void Update(Vector3 positionCamera)
        {
            world = Matrix.CreateConstrainedBillboard(position, positionCamera, Vector3.Up, Vector3.Forward, Vector3.Forward);

        }      
    }
}
