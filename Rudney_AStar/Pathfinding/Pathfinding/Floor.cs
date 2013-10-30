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
    public class Floor
    {
        Matrix world;

        VertexPositionTexture[] vertices;
        VertexBuffer vertexBuffer;

        BasicEffect effect;

        Texture2D texture;
        Camera camera;

        public Floor(Game game,Vector3 scale, Vector3 rotation, Vector3 position, Texture2D texture,Camera camera)
        {
            this.camera = camera;
            this.world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);

            this.texture = texture;

            Vector3[] verts = new Vector3[] 
            {
                new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 1),
                new Vector3(1, 0, -1),
                new Vector3(1,0,1)

            };

            int[] triangles = new int[] 
            {
                0,1,2,
                2,1,3
            };

            Vector2[] uv = new Vector2[] 
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 0),

                new Vector2(1,0),
                new Vector2(0, 1),
                new Vector2(1, 1)
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

        public void Draw(GraphicsDevice gd)
        {
            gd.RasterizerState = RasterizerState.CullNone;
            gd.SetVertexBuffer(vertexBuffer);
            gd.SamplerStates[0] = SamplerState.LinearClamp;

            effect.World = world;
            effect.View = camera.View ;
            effect.Projection = camera.Projection;

            effect.TextureEnabled = true;
            effect.Texture = this.texture;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gd.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
            }
        }
    }
}