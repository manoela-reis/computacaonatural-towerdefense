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

        private float timer = 0;
        private int timerInt = 0;

        private AStar path;

        public static List<Character> enemys;
        private static Character[] enemysArray;
        private Character player;

        KeyboardManager keyboard;

        private Grid grid;

        private Node activeNode;
        public List<Tree> towers;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            nodePath = new List<Node>();
            towers = new List<Tree>();
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

            ImageLibrary.getInstance().putImage("Tree", Content.Load<Texture2D>(@"tree"));
            ImageLibrary.getInstance().putImage("Clear", Content.Load<Texture2D>(@"clear"));
            ImageLibrary.getInstance().putImage("Floor", Content.Load<Texture2D>(@"ground"));
            ImageLibrary.getInstance().putImage("FloorSelected", Content.Load<Texture2D>(@"ground_selected"));
            ImageLibrary.getInstance().putImage("Origin", Content.Load<Texture2D>(@"origin"));
            ImageLibrary.getInstance().putImage("Player", Content.Load<Texture2D>(@"player"));

            grid = new Grid(this, cam, 6, 6);

            this.path = new AStar();

            Random rand = new Random();

            int x = rand.Next(3, 6);
            int y = rand.Next(3, 6);

            this.path.Search(this.grid.nodes[0, 0], this.grid.nodes[x, y], ref this.grid, this, ref allTrees, ref this.nodePath);
            ArrangeNodes(ref this.nodePath);

            player = new Character(this, new Vector3(1.25f, 0, 1.25f), new Vector3(0, 90, 0), new Vector3(0, 1, 0), ImageLibrary.getInstance().getImage("Player"), cam, path.origin);
            enemys = new List<Character>();
            enemys.Add(player);

            activeNode = grid.nodes[0, 0];
        }

        protected override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            timerInt = (int)timer;

            if (timerInt >= 5)
            {
                //Character p = new Character(this, new Vector3(1.25f, 0, 1.25f), new Vector3(0, 90, 0), new Vector3(0, 1, 0), ImageLibrary.getInstance().getImage("Player"), cam, path.origin);
                //enemys.Add(p);
                timer = 0;
            }

            this.keyboard.Update();

            enemysArray = enemys.ToArray();

            //Erro ao utilizar foreach no Update. Melhor Solução foi esta.
            for (int i = 0; i < enemysArray.Length; i++)
            {
                enemysArray[i].Update(gameTime);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (this.keyboard.IsKeyDown(Keys.P))
            {
                Random rand = new Random();

                int x = rand.Next(3, 6);
                int y = rand.Next(0, 6);

                this.path.Search(this.grid.nodes[0, 0], this.grid.nodes[x, y], ref this.grid, this, ref allTrees, ref this.nodePath);
                ArrangeNodes(ref this.nodePath);

                player.setPosition(new Vector3(0, 1, 0));
                player.target = path.origin;
            }

            if (this.keyboard.IsKeyDown(Keys.Space)) //Ação!
            {
                if (activeNode.state == nodeState.OPEN)
                {
                    towers.Add(new Tree(this, new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector3(activeNode.x * 2.5f, 0, activeNode.y * 2.5f), cam));
                }
            }

            if (this.keyboard.IsKeyDown(Keys.Down))
            {
                if (activeNode.y < 5)
                {
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("Floor"));
                    int x = activeNode.x;
                    int y = activeNode.y;

                    activeNode = grid.nodes[x, y + 1];
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("FloorSelected"));
                }
            }

            if (this.keyboard.IsKeyDown(Keys.Up))
            {
                if (activeNode.y > 0)
                {
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("Floor"));
                    int x = activeNode.x;
                    int y = activeNode.y;

                    activeNode = grid.nodes[x, y - 1];
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("FloorSelected"));
                }
            }

            if (this.keyboard.IsKeyDown(Keys.Right))
            {
                if (activeNode.x < 5)
                {
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("Floor"));
                    int x = activeNode.x;
                    int y = activeNode.y;

                    activeNode = grid.nodes[x + 1, y];
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("FloorSelected"));
                }
            }

            if (this.keyboard.IsKeyDown(Keys.Left))
            {
                if (activeNode.x > 0)
                {
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("Floor"));
                    int x = activeNode.x;
                    int y = activeNode.y;

                    activeNode = grid.nodes[x - 1, y];
                    activeNode.ChangeTexture(ImageLibrary.getInstance().getImage("FloorSelected"));
                }
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


            foreach (Character p in enemys)
                p.Draw(GraphicsDevice);

            grid.Draw(GraphicsDevice);

            ordenation.Sort(allTrees);

            foreach (Tree currentTree in allTrees)
            {
                currentTree.Draw(GraphicsDevice);
            }

            foreach (Tree tree in towers)
            {
                tree.Draw(GraphicsDevice);
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
