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
    public class Tree
    {
        public double module;

        Vector3 position;

        Matrix world;

        VertexPositionTexture[] vertices;
        VertexBuffer vertexBuffer;

        BasicEffect effect;

        Camera camera;

        Texture2D texture;

        public Tree(Game game, Vector3 scale, Vector3 rotation, Vector3 position,Camera cam)
        {
            this.world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);
           
            this.camera = cam;
            this.position = position;
            this.texture = ImageLibrary.getInstance().getImage("Tree");

            Vector3[] verts = new Vector3[] 
            {
                new Vector3(-1, 0, 0),
                new Vector3(-1, 2, 0),
                new Vector3(1, 2, 0),

                new Vector3(1, 2, 0),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0)

            };

            int[] triangles = new int[] 
            {
                0,1,2,
                3,4,5
            };

            Vector2[] uv = new Vector2[] 
            {
                new Vector2(0, 1),
                new Vector2(0, 0),
                new Vector2(1, 0),

                new Vector2(1,0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            int[] trianglesuv = new int[] 
            {
                0,1,2,
                3,4,5
            };

            ConvertMesh myMesh = new ConvertMesh();
            vertices = myMesh.returnTriangle(verts, triangles, uv, trianglesuv);

           
            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertices);

            effect = new BasicEffect(game.GraphicsDevice);
        }

        public void ChangeTexture(Texture2D tex)
        {
            this.texture = tex;
        }

        public void Update()
        {
            world = Matrix.Invert(camera.View);
            world.Translation = position;
            captureModule(camera.positions[0]);
        }

        public void Translate(Vector3 translate)
        {
            this.position += translate;
        }


        public void captureModule(Vector3 originPosition)
        {
            double a = Math.Abs(originPosition.X-position.X);
            double b = Math.Abs(originPosition.Z-position.Z);

            double squareRoot = Math.Pow(a, 2) + Math.Pow(b, 2);
            module = Math.Sqrt(squareRoot);    
        }

        public void Draw(GraphicsDevice gd)
        {
            gd.SetVertexBuffer(vertexBuffer);
            gd.SamplerStates[0] = SamplerState.LinearClamp;

            gd.BlendState = BlendState.AlphaBlend;

            effect.World = world;
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            effect.TextureEnabled = true;
            effect.Texture = this.texture;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gd.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }

            gd.BlendState = BlendState.Opaque;
        }
    }
}