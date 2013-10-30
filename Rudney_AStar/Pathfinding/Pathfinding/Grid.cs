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
    public class Grid
    {
        public Node[,] nodes;
        public int Rows;
        public int Columns;

        public Grid(Game game,Camera mainCamera,int Rows, int Columns)
        {
            this.Rows = Rows;
            this.Columns = Columns;

            nodes = new Node[Rows, Columns];

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    float positionX = 1 * x;
                    float positionY = 1 * y;
                    nodes[x, y] = new Node(game, new Vector3(1.25f, 0, 1.25f), new Vector3(0, 0, 0), new Vector3(positionX*2.5f, 0, positionY*2.5f),ImageLibrary.getInstance().getImage("Floor"), mainCamera);
                }
            }
        }

        public void Draw(GraphicsDevice gd)
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    nodes[x, y].Draw(gd);
                }
            }
        }
    }
}
