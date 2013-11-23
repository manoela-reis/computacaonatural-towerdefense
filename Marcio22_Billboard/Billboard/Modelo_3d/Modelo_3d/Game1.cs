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

namespace Modelo3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix view, projection, world, heliceRotation,worldTranslation, worldRotation;
        Model model;

        VertexPositionTexture[] moinho1;
        VertexPositionTexture[] moinho2;
        VertexPositionTexture[] casa;
        VertexPositionTexture[] vertsArvores;
        VertexPositionTexture[] plano;
        VertexPositionTexture[] verts;

        VertexBuffer vertexBuffer;
        BasicEffect effectW;
        BasicEffect effectH;
        Effect AdvancedEffect;

        Model[] dragon;

        Texture2D tDragon;

        Effect effect;
        int currentFrame;
        float time;
        float interSpeed;
        short[] index;
        float[][] buffer;
        int size;
        Vector2[] ct;

        private Texture2D texturaPlano;
        private Texture2D texturaCasa;
        private Texture2D texturaHelice;
        private Texture2D texturaBaseHelice;
        private Texture2D texturaArvore;

        private Vector3 posicao = new Vector3(15, 0, 70);
        private Vector3 posicaoPersonagem;

        private float rotacao = MathHelper.PiOver4 / 30;
        private float rotacaoCamera;
        private float speed = 1.2f;

        private Vector2[] posicoesTextura = new Vector2[4];
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.worldTranslation = Matrix.Identity; 
            this.worldRotation = Matrix.Identity;
            this.world = Matrix.Identity;
            this.heliceRotation = Matrix.Identity;
            this.speed = 1;
            this.currentFrame = 0;
            this.time = 0;

            this.view = Matrix.CreateLookAt(posicao,
            Vector3.Zero,
            Vector3.Up);

            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            Window.ClientBounds.Width /
            (float)Window.ClientBounds.Height,
            0.1f, 250);

            effectW = new BasicEffect(GraphicsDevice);
            effectH = new BasicEffect(GraphicsDevice); 

            plano = new VertexPositionTexture[4];
            plano[0] = new VertexPositionTexture(new Vector3(-100, -15, 100), new Vector2(0, 1));
            plano[1] = new VertexPositionTexture(new Vector3(100, -15, 100), new Vector2(1, 1));
            plano[2] = new VertexPositionTexture(new Vector3(-100, -15, -100), new Vector2(0, 0));
            plano[3] = new VertexPositionTexture(new Vector3(100, -15, -100), new Vector2(1, 0));

            //casa
            casa = new VertexPositionTexture[8];
            casa[0] = new VertexPositionTexture(new Vector3(10, -15, -10), new Vector2(0, 1));
            casa[1] = new VertexPositionTexture(new Vector3(-10, -15, -10), new Vector2(1, 1));
            casa[2] = new VertexPositionTexture(new Vector3(10, -8, -10), new Vector2(0, 0));
            casa[3] = new VertexPositionTexture(new Vector3(-10, -8, -10), new Vector2(1, 0));
            casa[4] = new VertexPositionTexture(new Vector3(10, -8, -30), new Vector2(0, 1));
            casa[5] = new VertexPositionTexture(new Vector3(-10, -8, -30), new Vector2(1, 1));
            casa[6] = new VertexPositionTexture(new Vector3(10, -15, -30), new Vector2(0, 0));
            casa[7] = new VertexPositionTexture(new Vector3(-10, -15, -30), new Vector2(1, 0));

            moinho1 = new VertexPositionTexture[16];
            //centro
            moinho1[0] = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            moinho1[1] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            moinho1[2] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1));

            //top left
            moinho1[3] = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            moinho1[4] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            moinho1[5] = new VertexPositionTexture(new Vector3(-9, 9, 0), new Vector2(0, 1));

            //top right
            moinho1[6] = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            moinho1[7] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1));
            moinho1[8] = new VertexPositionTexture(new Vector3(9, 9, 0), new Vector2(0, 1));

            //under middle
            moinho1[9] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            moinho1[10] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1));
            moinho1[11] = new VertexPositionTexture(new Vector3(0, -13, 0), new Vector2(0, 1));
            //base
            moinho1[12] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 1));
            moinho1[13] = new VertexPositionTexture(new Vector3(-1, -18, -1), new Vector2(1, 1));
            moinho1[14] = new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(0, 0));
            moinho1[15] = new VertexPositionTexture(new Vector3(1, -18, -1), new Vector2(1, 0));

            // moinho2

            moinho2 = new VertexPositionTexture[16];
            //centro
            moinho2[0] = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            moinho2[1] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            moinho2[2] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1));

            //top left
            moinho2[3] = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            moinho2[4] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            moinho2[5] = new VertexPositionTexture(new Vector3(-9, 9, 0), new Vector2(0, 1));

            //top right
            moinho2[6] = new VertexPositionTexture(new Vector3(0, 1, 0), new Vector2(0, 1));
            moinho2[7] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1));
            moinho2[8] = new VertexPositionTexture(new Vector3(9, 9, 0), new Vector2(0, 1));

            //under middle
            moinho2[9] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1));
            moinho2[10] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(0, 1));
            moinho2[11] = new VertexPositionTexture(new Vector3(0, -13, 0), new Vector2(0, 1));
            //base
            moinho2[12] = new VertexPositionTexture(new Vector3(-1, -1, -1), new Vector2(0, 1));
            moinho2[13] = new VertexPositionTexture(new Vector3(-1, -18, -1), new Vector2(1, 1));
            moinho2[14] = new VertexPositionTexture(new Vector3(1, -1, -1), new Vector2(0, 0));
            moinho2[15] = new VertexPositionTexture(new Vector3(1, -18, -1), new Vector2(1, 1));

            // arvore billboard
            vertsArvores = new VertexPositionTexture[4];
            vertsArvores[0] = new VertexPositionTexture(new Vector3(5, -15, -2), new Vector2(0, 1));
            vertsArvores[1] = new VertexPositionTexture(new Vector3(-5, -15, -2), new Vector2(1, 1));
            vertsArvores[2] = new VertexPositionTexture(new Vector3(5, -8, -2), new Vector2(1, 0));
            vertsArvores[3] = new VertexPositionTexture(new Vector3(-5, -8, -2), new Vector2(0, 1));

            posicoesTextura[0] = new Vector2(0, 1);
            posicoesTextura[1] = new Vector2(1, 1);
            posicoesTextura[2] = new Vector2(0, 0);
            posicoesTextura[3] = new Vector2(1, 0);
            for (int i = 0; i < vertsArvores.Length; i++)
            {
                vertsArvores[i] = new VertexPositionTexture(new Vector3(vertsArvores[i].Position.X,vertsArvores[i].Position.Y,vertsArvores[i].Position.Z), posicoesTextura[i]);
            }
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                plano.Length, BufferUsage.None);
            vertexBuffer.SetData(plano); 
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                 moinho1.Length, BufferUsage.None);
            vertexBuffer.SetData(moinho1); 
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                  moinho2.Length, BufferUsage.None);
            vertexBuffer.SetData(moinho2); 
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                   vertsArvores.Length, BufferUsage.None);
            vertexBuffer.SetData(vertsArvores);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.model = Content.Load<Model>(@"Models\spaceship"); 
            this.texturaPlano = Content.Load<Texture2D>(@"Textures\Matinho");
            this.texturaCasa = Content.Load<Texture2D>(@"Textures\Parede");
            this.texturaHelice = Content.Load<Texture2D>(@"Textures\Helice");
            this.texturaBaseHelice = Content.Load<Texture2D>(@"Textures\BaseHelice");
            this.texturaArvore = Content.Load<Texture2D>(@"Textures\Arvore");

            this.AdvancedEffect = Content.Load<Effect>(@"Effect\effect");
            this.dragon = new Model[8];

            for (int i = 0; i < this.dragon.Length; i++)
                this.dragon[i] = Content.Load<Model>(@"Models\Dragon\" + (i + 1));

            this.tDragon = Content.Load<Texture2D>(@"Textures\dragon");

            this.effect = Content.Load<Effect>(@"Effect\Effect1");

            this.size = this.dragon[0].Meshes[0].MeshParts[0].VertexBuffer.VertexCount;
            this.buffer = new float[this.dragon.Length][];
            for (int i = 0; i < this.dragon.Length; i++)
            {
                this.buffer[i] = new float[this.size * 8];
                this.dragon[i].Meshes[0].MeshParts[0].VertexBuffer.GetData<float>(this.buffer[i]);
            }

            int idx = this.dragon[0].Meshes[0].MeshParts[0].IndexBuffer.IndexCount;
            this.index = new short[idx];
            this.dragon[0].Meshes[0].MeshParts[0].IndexBuffer.GetData<short>(index);

            this.verts = new VertexPositionTexture[this.size];
            this.vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                this.verts.Length, BufferUsage.None);
            this.vertexBuffer.SetData<VertexPositionTexture>(this.verts);

            this.ct = new Vector2[this.size];

            int k = this.currentFrame;
            int l = this.currentFrame + 1;

            if (l == this.dragon.Length)
                l = 0;

            for (int i = 0; i < this.size; i++)
            {
                int j = i * 8;

                Vector3 position = new Vector3(this.buffer[k][j], -this.buffer[k][j + 2], -this.buffer[k][j + 1]);

                this.ct[i] = new Vector2(this.buffer[0][j + 6], this.buffer[0][j + 7]);

                this.verts[i] = new VertexPositionTexture(position, this.ct[i]);
            }           
        }

        protected override void UnloadContent()
        {
            this.texturaArvore.Dispose();
            this.texturaBaseHelice.Dispose();
            this.texturaCasa.Dispose();
            this.texturaHelice.Dispose();
            this.texturaPlano.Dispose();
            this.AdvancedEffect.Dispose();
            this.dragon = null;
            this.tDragon.Dispose();
            this.effect.Dispose();
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (keyboardState.IsKeyDown(Keys.OemPlus))
                this.interSpeed += 0.1f;
            if (keyboardState.IsKeyDown(Keys.OemMinus))
                if (this.interSpeed > 0.2f)
                    this.interSpeed -= 0.1f;

            this.time += gameTime.ElapsedGameTime.Milliseconds * 0.001f * this.interSpeed;
            if (this.time > 1)
            {
                this.time = 0;
                this.currentFrame++;
                if (this.currentFrame >= this.dragon.Length)
                    this.currentFrame = 0;
            }

            int k = this.currentFrame;
            int l = this.currentFrame + 1;

            if (l == this.dragon.Length)
                l = 0;

            for (int i = 0; i < this.size; i++)
            {
                int j = i * 8;

                Vector3 position = new Vector3(this.buffer[k][j],
                                              -this.buffer[k][j + 2],
                                              -this.buffer[k][j + 1]) * (1 - this.time) +
                                   new Vector3(this.buffer[l][j],
                                              -this.buffer[l][j + 2],
                                              -this.buffer[l][j + 1]) * this.time;

                this.verts[i] = new VertexPositionTexture(position, this.ct[i]);
            }

            this.Input();
            this.heliceRotation *= Matrix.CreateRotationZ(MathHelper.PiOver4 / 30);
            this.view = Matrix.CreateLookAt(posicao, posicaoPersonagem, Vector3.Up);
            base.Update(gameTime);
        }
        private void Input()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                worldRotation *= Matrix.CreateRotationY(-rotacao);
                rotacaoCamera = -rotacao;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                worldRotation *= Matrix.CreateRotationY(rotacao);
                rotacaoCamera = rotacao;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //worldTranslation *= Matrix.CreateTranslation(new Vector3(-speed,0,0));

                posicaoPersonagem = new Vector3(posicaoPersonagem.X + (Vector3.Left.X * speed), posicaoPersonagem.Y, posicaoPersonagem.Z);
                worldTranslation *= Matrix.CreateTranslation(new Vector3(Vector3.Left.X * speed, 0, 0));
                posicao = new Vector3(posicao.X + (Vector3.Left.X * speed), posicao.Y, posicao.Z);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //worldTranslation *= Matrix.CreateTranslation(new Vector3(speed, 0, 0));
                posicaoPersonagem = new Vector3(posicaoPersonagem.X + (Vector3.Right.X * speed), posicaoPersonagem.Y, posicaoPersonagem.Z);
                worldTranslation *= Matrix.CreateTranslation(new Vector3(Vector3.Right.X * speed, 0, 0));
                posicao = new Vector3(posicao.X + (Vector3.Right.X * speed), posicao.Y, posicao.Z);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                posicaoPersonagem = new Vector3(posicaoPersonagem.X, posicaoPersonagem.Y, posicaoPersonagem.Z + (Vector3.Forward.Z * speed));
                worldTranslation *= Matrix.CreateTranslation(new Vector3(0, 0, Vector3.Forward.Z * speed));

                posicao = new Vector3(posicao.X, posicao.Y, posicao.Z + (Vector3.Forward.Z * speed));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                //worldTranslation *= Matrix.CreateTranslation(new Vector3(0, 0, Vector3.Backward.Z * speed));
                posicaoPersonagem = new Vector3(posicaoPersonagem.X, posicaoPersonagem.Y, posicaoPersonagem.Z + (Vector3.Backward.Z * speed));
                worldTranslation *= Matrix.CreateTranslation(new Vector3(0, 0, Vector3.Backward.Z * speed));
                posicao = new Vector3(posicao.X, posicao.Y, posicao.Z + (Vector3.Backward.Z * speed));
                //posicao = new Vector3(posicao.X, posicao.Y, posicao.Z);
            }
            //view *= Matrix.CreateRotationY(rotacao);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            AdvancedEffect.CurrentTechnique = AdvancedEffect.Techniques["Technique1"];
            AdvancedEffect.Parameters["World"].SetValue(world);
            AdvancedEffect.Parameters["View"].SetValue(view); 
            AdvancedEffect.Parameters["Projection"].SetValue(projection);
            AdvancedEffect.Parameters["colorTexture"].SetValue(this.texturaArvore);
            
            this.effect.CurrentTechnique = this.effect.Techniques["Technique1"];
            this.effect.Parameters["Projection"].SetValue(this.projection);
            this.effect.Parameters["View"].SetValue(this.view);
            this.effect.Parameters["World"].SetValue(this.world);
            this.effect.Parameters["colorTexture"].SetValue(this.tDragon);

            foreach (EffectPass pass in this.effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList,
                    this.verts, 0, this.verts.Length, this.index, 0, this.index.Length / 3);
            }


            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = worldRotation * worldTranslation * mesh.ParentBone.Transform;
                    effect.View = this.view;
                    effect.Projection = this.projection;
                }
            mesh.Draw();
            }
            foreach (EffectPass pass in AdvancedEffect.CurrentTechnique.Passes)
            {
                AdvancedEffect.CurrentTechnique = AdvancedEffect.Techniques["Technique1"];
                AdvancedEffect.Parameters["World"].SetValue(Matrix.CreateBillboard(new Vector3(30, 0, 0), posicao, Vector3.Up, Vector3.Forward));
                AdvancedEffect.Parameters["View"].SetValue(view);
                AdvancedEffect.Parameters["Projection"].SetValue(projection);
                AdvancedEffect.Parameters["colorTexture"].SetValue(this.texturaArvore);
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertsArvores, 0, 2);
            }
            foreach (EffectPass pass in effectW.CurrentTechnique.Passes)
            {
                effectW.World = this.world;
                effectW.View = this.view;
                effectW.Projection = this.projection;
                effectW.TextureEnabled = true;
                effectW.Texture = this.texturaPlano;
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, plano, 0, 2);

                effectW.Texture = this.texturaCasa;
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, casa, 0, 6);
            } 
            foreach (EffectPass pass in effectH.CurrentTechnique.Passes)
            {
                effectH.World = this.heliceRotation * Matrix.CreateTranslation(-15, 0, 0);
                effectH.View = this.view;
                effectH.Projection = this.projection;
                effectH.TextureEnabled = true; 
                effectW.Texture = this.texturaHelice;
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, moinho1, 0, 4);

                effectH.World = this.heliceRotation * Matrix.CreateTranslation(15, 0, 0);
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, moinho2, 0, 4);
                effectH.World = Matrix.CreateTranslation(-15, 0, 0);
                effectW.Texture = this.texturaBaseHelice;
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, moinho1, 12, 2);
                effectH.World = Matrix.CreateTranslation(15, 0, 0);
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, moinho2, 12, 2);
            }

            base.Draw(gameTime);
        }
    }
}
