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
    public class AStar
    {
        Node[] trees;

        Point[] _movements;
        Node[,] _squares;
        public Node origin;
        Game1 game;

        List<Node> allNodes;
        List<Node> path;

        private int row;
        private int column;

        public AStar()
        {
            allNodes = new List<Node>();
                _movements = new Point[]
                {
                    new Point(0, -1),
                    new Point(1, 0),
                    new Point(0, 1),
                    new Point(-1, 0)
                };
        }

        public void Search(Node origin, Node goal, ref Grid grid, Game1 game, ref Tree[] allTrees, ref List<Node> _path)
        {
            this.path = _path;
            this.game = game;

            allNodes.Clear();
            
            ResetNodes();

            this.origin = origin;
            this._squares = grid.nodes;
            this.row = grid.Rows;
            this.column = grid.Columns;



            foreach (Point p in GetPoints())
            {
                allNodes.Add(_squares[p.X,p.Y]);
                _squares[p.X, p.Y].ChangeTexture(ImageLibrary.getInstance().getImage("Floor"));
            }

            allNodes.Remove(goal);
            allNodes.Remove(origin);

            foreach (Node tree in SortTrees(6))
            {
                tree.state = nodeState.CLOSED;
            }

            goal.state = nodeState.GOAL;
            origin.state = nodeState.ORIGIN;


            Pathfind();
            DrawPath(goal,ref grid,ref allTrees);

        }

        public void ResetNodes()
        {
            foreach (Point point in GetPoints())
            {
                int x = point.X;
                int y = point.Y;
                _squares[x, y].distanceSteps = 10000;
                _squares[x, y].hasPath = false;
                _squares[x, y].state = nodeState.OPEN;
            }
        }

        public void Pathfind()
        {
            Point startingPoint = StateToPoint(nodeState.GOAL);
            int heroX = startingPoint.X;
            int heroY = startingPoint.Y;
            if (heroX == -1 || heroY == -1)
            {
                return;
            }

            _squares[heroX, heroY].distanceSteps = 0;

            while (true)
            {
                bool madeProgress = false;

                foreach (Point mainPoint in GetPoints())
                {
                    int x = mainPoint.X;
                    int y = mainPoint.Y;

                    if (SquareOpen(x, y))
                    {
                        int passHere = _squares[x, y].distanceSteps;

                        foreach (Point movePoint in CheckStep(x, y))
                        {
                            int newX = movePoint.X;
                            int newY = movePoint.Y;
                            int newPass = passHere + 1;

                            if (_squares[newX, newY].distanceSteps > newPass)
                            {
                                _squares[newX, newY].distanceSteps = newPass;
                                madeProgress = true;
                            }
                        }
                    }
                }
                if (!madeProgress)
                {
                    break;
                }
            }
        }

        private bool CheckMove(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }
            
            if (x > this.column - 1 || y > this.row - 1)
            {
                return false;
            }
            
            return true;
        }

        private bool SquareOpen(int x, int y)
        {

            switch (_squares[x, y].state)
            {
                case nodeState.OPEN:
                    return true;
                case nodeState.GOAL:
                    return true;
                case nodeState.ORIGIN:
                    return true;
                case nodeState.CLOSED:
                default:
                    return false;
            }
        }

        private Point StateToPoint(nodeState contentIn)
        {

            foreach (Point point in GetPoints())
            {
                if (_squares[point.X, point.Y].state == contentIn)
                {
                    return new Point(point.X, point.Y);
                }
            }
            return new Point(-1, -1);
        }

        public void DrawPath(Node goal,ref Grid grid,ref Tree[] _trees)
        {

            Point startingPoint = StateToPoint(nodeState.ORIGIN);

            int pointX = startingPoint.X;
            int pointY = startingPoint.Y;

            path.Add(origin);

            if (pointX == -1 && pointY == -1)
            {
                return;
            }

            while (true)
            {

                Point lowestPoint = Point.Zero;
                int lowest = 10000;

                foreach (Point movePoint in CheckStep(pointX, pointY))
                {
                    int count = _squares[movePoint.X, movePoint.Y].distanceSteps;
                    if (count < lowest)
                    {
                        lowest = count;
                        lowestPoint.X = movePoint.X;
                        lowestPoint.Y = movePoint.Y;
                    }
                }
                if (lowest != 10000)
                {
                    _squares[lowestPoint.X, lowestPoint.Y].hasPath = true;
                    _squares[lowestPoint.X, lowestPoint.Y].ChangeTexture(ImageLibrary.getInstance().getImage("Clear"));
                    path.Add(_squares[lowestPoint.X, lowestPoint.Y]);
                    pointX = lowestPoint.X;
                    pointY = lowestPoint.Y;
                }
                else
                {
                    Search(origin, goal, ref grid, this.game, ref _trees, ref this.path);
                    return;
                }

                if (_squares[pointX, pointY].state == nodeState.GOAL)
                {
                    _squares[pointX, pointY].ChangeTexture(ImageLibrary.getInstance().getImage("Origin"));
                    origin.ChangeTexture(ImageLibrary.getInstance().getImage("Origin"));
                    path.Add(_squares[pointX, pointY]);
                    CreateTrees(ref _trees);
                    break;
                }
            }
        }

        private void CreateTrees(ref Tree[] _trees)
        { 
            int length = trees.Length;
            _trees = new Tree[length];

            for (int i = 0; i < length; i++)
            {
                _trees[i] = new Tree(game, new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector3(trees[i].getPosition().X, 0, trees[i].getPosition().Z), game.cam);
            }
        }

        private IEnumerable<Point> GetPoints()
        {

            for (int x = 0; x < this.column; x++)
            {
                for (int y = 0; y < this.row; y++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private IEnumerable<Point> CheckStep(int x, int y)
        {
            foreach (Point movePoint in _movements)
            {
                int newX = x + movePoint.X;
                int newY = y + movePoint.Y;

                if (CheckMove(newX, newY) &&
                    SquareOpen(newX, newY))
                {
                    yield return new Point(newX, newY);
                }
            }
        }

        private Node[] SortTrees(int value)
        {
            trees = new Node[value];

            Random random = new Random();

            for (int i = 0; i < value; i++)
            {
                trees[i] = allNodes[random.Next(allNodes.Count)];
                allNodes.Remove(trees[i]);
            }
            
            return trees;
        }

    }
}
