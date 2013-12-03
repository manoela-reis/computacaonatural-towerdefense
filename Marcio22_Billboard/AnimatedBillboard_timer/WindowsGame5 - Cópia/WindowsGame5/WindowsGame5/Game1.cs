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

namespace WindowsGame5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D textureMoinho;
        Texture2D textureArvore;
        Texture2D textureGrama;

        Animacao animacao;

        VertexPositionTexture[] vertsMoinho;
        VertexPositionTexture[] vertsHelice;
        VertexPositionTexture[] vertsGrama;

        VertexBuffer vertexBuffer;
        VertexBuffer vertexBuffer2;
        VertexBuffer vertexBufferGrama;

        Matrix world;
        Matrix world2;
        Matrix view;
        Matrix projection;
        BasicEffect effectBasic;
        Effect effectShader;

        Model model;


        Vector3 position;

        Vector2 maxTela;
        Vector2 posicaoMouse;
        Vector2 meioTela;
        Vector2 aceleracaoMouse;

        Vector2 rotacaoCamera;

        Arvore[] arvore = new Arvore[25];



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            maxTela = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            //world = Matrix.Identity;
            world = Matrix.CreateTranslation(new Vector3(0.75f, -2.5f, 0));
            world2 = Matrix.CreateConstrainedBillboard(new Vector3(0, -2, -0.1f), new Vector3(0, 0, 0), Vector3.Up, Vector3.Forward, Vector3.Forward);//Matrix.CreateTranslation(new Vector3(0, -2, -0.1f));

            position = new Vector3(0, 0, -15);

            meioTela = new Vector2((int)maxTela.X / 2, (int)maxTela.Y / 2);

            view = Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            Window.ClientBounds.Width /
            (float)Window.ClientBounds.Height,
            1, 100);

            posicaoMouse = rotacaoCamera = Vector2.Zero;

            int contador = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    arvore[contador] = new Arvore(new Vector3(i * 2 - 5, -2, j * 2 - 5), position, GraphicsDevice);
                    contador++;
                }
            }

            vertsMoinho = new VertexPositionTexture[10];
            vertsMoinho[0] = new VertexPositionTexture(new Vector3(-1, 3, 0), new Vector2(0, 0));
            vertsMoinho[1] = new VertexPositionTexture(new Vector3(-0.5f, 3, 0), new Vector2(1, 0));
            vertsMoinho[2] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(1, 1));
            vertsMoinho[3] = new VertexPositionTexture(new Vector3(-0.5f, -1, 0), new Vector2(1, 1));
            // Embaixo
            /*/vertsMoinho[4] = new VertexPositionTexture(new Vector3(-1, -1, 0.5f), new Vector2(1, 1));
            vertsMoinho[5] = new VertexPositionTexture(new Vector3(-0.5f, -1, 0.5f), new Vector2(1, 1));
            // Atrás
            vertsMoinho[6] = new VertexPositionTexture(new Vector3(-1, 3, 0.5f), new Vector2(1, 1));
            vertsMoinho[7] = new VertexPositionTexture(new Vector3(-0.5f, 3, 0.5f), new Vector2(1, 1));
            // Acima
            vertsMoinho[8] = new VertexPositionTexture(new Vector3(-1, 3, 0), new Vector2(1, 1));
            vertsMoinho[9] = new VertexPositionTexture(new Vector3(-0.5f, 3, 0), new Vector2(1, 1));/*/


            vertsHelice = new VertexPositionTexture[9];
            vertsHelice[0] = new VertexPositionTexture(new Vector3(-2, 3, 0), new Vector2(0, 1));
            vertsHelice[1] = new VertexPositionTexture(new Vector3(-3, 2.5f, 0), new Vector2(0, 0));
            vertsHelice[2] = new VertexPositionTexture(new Vector3(0, 2, 0), new Vector2(1, 1));

            vertsHelice[3] = new VertexPositionTexture(new Vector3(2, 3, 0), new Vector2(0, 1));
            vertsHelice[4] = new VertexPositionTexture(new Vector3(3, 2.5f, 0), new Vector2(0, 0));
            vertsHelice[5] = new VertexPositionTexture(new Vector3(0, 2, 0), new Vector2(1, 1));

            vertsHelice[6] = new VertexPositionTexture(new Vector3(0, 2, 0), new Vector2(0, 1));
            vertsHelice[7] = new VertexPositionTexture(new Vector3(0.5f, -0.75f, 0), new Vector2(0, 0));
            vertsHelice[8] = new VertexPositionTexture(new Vector3(-0.5f, -0.75f, 0), new Vector2(1, 1));


            vertsGrama = new VertexPositionTexture[4];
            vertsGrama[0] = new VertexPositionTexture(new Vector3(-10, -1, 8), new Vector2(0, 0));
            vertsGrama[1] = new VertexPositionTexture(new Vector3(10, -1, 8), new Vector2(1, 0));
            vertsGrama[2] = new VertexPositionTexture(new Vector3(-10, -1, -12), new Vector2(0, 1));
            vertsGrama[3] = new VertexPositionTexture(new Vector3(10, -1, -12), new Vector2(1, 1));


            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                vertsMoinho.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertsMoinho);

            vertexBuffer2 = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                vertsHelice.Length, BufferUsage.None);
            vertexBuffer2.SetData<VertexPositionTexture>(vertsHelice);

            vertexBufferGrama = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture),
                vertsGrama.Length, BufferUsage.None);
            vertexBufferGrama.SetData<VertexPositionTexture>(vertsGrama);
           


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            effectBasic = new BasicEffect(GraphicsDevice);
            textureMoinho = Content.Load<Texture2D>(@"Texturas\Moinho");
            textureArvore = Content.Load<Texture2D>(@"Texturas\Arvore");
            textureGrama = Content.Load<Texture2D>(@"Texturas\Grama");
            model = Content.Load<Model>(@"Models\spaceship");
            effectShader = Content.Load<Effect>(@"Effects\effect");

            animacao = new Animacao(1, 2);
            animacao.textures[0] = textureMoinho;
            animacao.textures[1] = textureArvore;
            animacao.texture = animacao.textures[0];
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //world *= Matrix.CreateRotationX(-0.01f);
            posicaoMouse = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            aceleracaoMouse = (posicaoMouse - meioTela) / 10;

            world2 *= Matrix.CreateRotationZ(-0.01f);

            rotacaoCamera += aceleracaoMouse;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                this.position.X += 0.5f;
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
                this.position.X -= 0.5f;
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                this.position.Z += 0.5f;
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
                this.position.Z -= 0.5f;

            for (int i = 0; i < 25; i++)
            {
                arvore[i].Update(position);
            }

            BubbleSort(arvore);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //position.Z += aceleracaoMouse.Y;

            //view = Matrix.CreateLookAt(position, Vector3.Zero, Vector3.Up);
            view = Matrix.CreateTranslation(position);
            view *= Matrix.CreateRotationY(MathHelper.ToRadians(rotacaoCamera.X));
            view *= Matrix.CreateRotationX(MathHelper.ToRadians(rotacaoCamera.Y));

            // TODO: Add your update logic here
            Mouse.SetPosition((int)maxTela.X / 2, (int)maxTela.Y / 2);
            base.Update(gameTime);
        }

        void BubbleSort(Arvore[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (Vector3.Distance(position, array[i].position) > Vector3.Distance(position, array[j].position))
                    {
                        Arvore temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effectBasic.World = this.world;
            effectBasic.View = this.view;
            effectBasic.Projection = this.projection;
            //effect.VertexColorEnabled = true;
            effectBasic.TextureEnabled = true;
            effectBasic.Texture = textureMoinho;
          
            RasterizerState r = new RasterizerState();
            r.FillMode = FillMode.Solid;

            GraphicsDevice.RasterizerState = r;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;


            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effectM in mesh.Effects)
                {
                    effectM.EnableDefaultLighting();
                    effectM.World = this.world * mesh.ParentBone.Transform + Matrix.CreateTranslation(new Vector3(30, 0, -10));
                    effectM.World *= Matrix.CreateScale(0.3f);
                    effectM.View = this.view;
                    effectM.Projection = this.projection;
                }
                mesh.Draw();
            }

            /*/foreach (EffectPass pass in effectBasic.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                (PrimitiveType.TriangleStrip, vertsMoinho, 0, 2);
            }/*/

            


            effectBasic.World = this.world * Matrix.CreateTranslation(Vector3.Forward * 5);
            foreach (EffectPass pass in effectBasic.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                (PrimitiveType.TriangleStrip, vertsMoinho, 0, 2);
            }

            #region Grama
            effectBasic.World = this.world;
            effectBasic.Texture = textureGrama;
            foreach (EffectPass pass in effectBasic.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                (PrimitiveType.TriangleStrip, vertsGrama, 0, 2);
            }
            #endregion


            effectShader.CurrentTechnique = effectShader.Techniques["Technique1"];
            effectShader.Parameters["World"].SetValue(world2);
            effectShader.Parameters["View"].SetValue(view);
            effectShader.Parameters["Projection"].SetValue(projection);
            effectShader.Parameters["colorTexture"].SetValue(textureMoinho);

            effectBasic.Texture = textureMoinho;
            effectBasic.World = this.world2;
            foreach (EffectPass pass in effectBasic.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                (PrimitiveType.TriangleList, vertsHelice, 0, 3);
            }

            effectBasic.World = this.world2 * Matrix.CreateTranslation(Vector3.Forward * 5);
            foreach (EffectPass pass in effectBasic.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                (PrimitiveType.TriangleList, vertsHelice, 0, 3);
            }

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            animacao.Update(gameTime);
            effectBasic.Texture = animacao.texture;
            for (int i = 0; i < 25; i++)
            {
                effectBasic.World = arvore[i].world;
                foreach (EffectPass pass in effectBasic.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                    (PrimitiveType.TriangleStrip, arvore[i].verts, 0, 2);
                }
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
