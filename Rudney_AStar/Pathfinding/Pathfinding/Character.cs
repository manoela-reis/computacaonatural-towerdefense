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
    public class Character
    {
        Matrix world;

        public Node target;

        VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;

        BasicEffect effect;

        float speed = 3;

        Texture2D texture;
        Camera camera;

        Vector3 position;
        Vector3 rotation;
        Vector3 scale;

        public Character(Game game, Vector3 scale, Vector3 rotation, Vector3 position, Texture2D texture, Camera camera, Node target)
        {
            this.scale = scale;
            this.target = target;
            this.camera = camera;
            this.world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateTranslation(position);
            this.position = position;
            this.rotation = rotation;
            this.texture = texture;

            Vector3[] _verts = new Vector3[] 
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
            verts = myMesh.returnTriangle(_verts, triangles, uv, trianglesuv);


            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(verts);

            effect = new BasicEffect(game.GraphicsDevice);

            
        }

        public void ChangeTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            Walk(this.target, gameTime);

            world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(this.position);
        }

        public void Walk(Node target, GameTime gameTime)
        {
            if (target.getPosition().X > position.X)
            {
                //rotation.X = 180;
            }
            else
            {
                rotation.X = 0;
            }   

            if (Vector3.Distance(new Vector3(this.position.X ,this.position.Y, this.position.Z), target.getPosition()) > position.Y)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

                this.position.X += (target.getPosition().X - this.position.X) * dt * speed;
                this.position.Z += (target.getPosition().Z - this.position.Z) * dt * speed;
            }
            else
                if(Vector3.Distance(new Vector3(this.position.X ,this.position.Y, this.position.Z), target.getPosition()) <= position.Y + 0.1f)
            {
                this.target = this.target.nextNode;
            }
        }

        public void Draw(GraphicsDevice grap)
        {
            grap.RasterizerState = RasterizerState.CullNone;
            grap.SamplerStates[0] = SamplerState.LinearClamp;
            grap.SetVertexBuffer(vertexBuffer);

            effect.World = world;
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            effect.TextureEnabled = true;
            effect.Texture = this.texture;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                grap.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, verts.Length / 3);
            }
        }

        public Vector3 getPosition()
        {
            return this.position;
        }

        public void setPosition(Vector3 pos)
        {
            this.position = pos;
        }
    }


}
