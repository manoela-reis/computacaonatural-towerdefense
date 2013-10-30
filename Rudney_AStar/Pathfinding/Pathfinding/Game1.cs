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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sort ordenation;

        public Camera cam;
        

        public Tree[] allTrees;

        public List<Node> nodePath;
      
        private AStar path;

        private Character player;

        KeyboardManager keyboard;

        private Grid grid;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            nodePath = new List<Node>();
            this.keyboard = new KeyboardManager();
            this.allTrees = new Tree[0];

            this.IsMouseVisible = true;

            cam = new Camera(this, new Vector3(6, 4, 22));
           
            base.Initialize();
        }

        protected override void LoadContent()
        {

            ordenation = new QuickSort();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            ImageLibrary.getInstance().putImage("Tree",Content.Load<Texture2D>(@"tree"));
            ImageLibrary.getInstance().putImage("Clear", Content.Load<Texture2D>(@"clear"));
            ImageLibrary.getInstance().putImage("Blank", Content.Load<Texture2D>(@"blank"));
            ImageLibrary.getInstance().putImage("Origin", Content.Load<Texture2D>(@"origin"));
            ImageLibrary.getInstance().putImage("Player", Content.Load<Texture2D>(@"player"));

            grid = new Grid(this, cam, 6, 6);

            this.path = new AStar();

            Random rand = new Random();

            int x = rand.Next(3, 6);
            int y = rand.Next(3, 6);

            this.path.Search(this.grid.nodes[0, 0], this.grid.nodes[x,y], ref this.grid, this, ref allTrees, ref this.nodePath);
            ArrangeNodes(ref this.nodePath);

            player = new Character(this, new Vector3(1.25f, 0, 1.25f), new Vector3(0, 90, 0), new Vector3(0, 1, 0), ImageLibrary.getInstance().getImage("Player"), cam, path.origin);
            
        }

        

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            this.keyboard.Update();
            this.player.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (this.keyboard.IsKeyDown(Keys.P))
            {
                Random rand = new Random();

                int x = rand.Next(3, 6);
                int y = rand.Next(0, 6);

                this.path.Search(this.grid.nodes[0, 0], this.grid.nodes[x, y], ref this.grid, this, ref allTrees, ref this.nodePath);
                ArrangeNodes(ref this.nodePath);

                player.setPosition(new Vector3(0, 0.1f, 0));
                player.target = path.origin;
            }

            foreach (Tree currentTree in allTrees)
            {
                currentTree.Update();
            }

            cam.Update();


            this.keyboard.LateUpdate();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            if (cam.boleanaaa == true)
            {
                GraphicsDevice.Clear(Color.DarkMagenta);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }


            this.player.Draw(GraphicsDevice);

            grid.Draw(GraphicsDevice);

            ordenation.Sort(allTrees);

            foreach (Tree currentTree in allTrees)
            {
               currentTree.Draw(GraphicsDevice);
            }
            
            base.Draw(gameTime);
        }

        public void ArrangeNodes(ref List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                nodes[i].nextNode = nodes[i + 1];
            }
        }
    }
}
