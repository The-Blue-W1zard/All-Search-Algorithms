﻿namespace DIjkstras_Search
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;

    internal class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine("Hello, World!");

            int[,] maze = new int[30, 50];
            int[,] maze1 = new int[30, 50];
            int[,] maze2 = new int[30, 50];
            int[,] maze3 = new int[30, 50];
            int[,] maze4 = new int[30, 50];
            int[,] maze5 = new int[30, 50];







            //maze1[0, 0] = 2;
            //maze1[29, 49] = 3;
            //maze2[0, 0] = 2;
            //maze2[29, 49] = 3;
            Point startCell = new(0, 0);
            Point endCell = new(28, 48);
            maze1 = p.GenerateRowsCols(maze1);
            //maze2 = p.GenerateRowsCols(maze2);
            //maze3 = p.GenerateRowsCols(maze3);
            //maze4 = p.GenerateRowsCols(maze4);
            //maze5 = p.GenerateRowsCols(maze5);
          

            p.RandomizedDFSV2(maze1, startCell);
            //p.RandomizedDFSV2(maze2, startCell);
            //p.RandomizedDFSV2(maze3, startCell);
            //p.RandomizedDFSV2(maze4, startCell);
            //p.RandomizedDFSV2(maze5, startCell);



            p.OutputMaze(maze1);
            //Console.WriteLine();
            //p.OutputMaze(maze2);
            //Console.WriteLine();
            //p.OutputMaze(maze3);
            //Console.WriteLine();
            //p.OutputMaze(maze4);
            //Console.WriteLine();
            //p.OutputMaze(maze5);

            List<Point> path = p.AStarSearch(maze1, startCell, endCell);
            Console.WriteLine("A Star");
            Array.Copy(p.UpdateMaze(maze1, path), maze, maze.Length);
            p.OutputMaze(maze);

            path = p.DijkstraVersion2(maze1, startCell, endCell);
            Console.WriteLine("Dijkstras");
            Array.Copy(p.UpdateMaze(maze1, path), maze, maze.Length);
            p.OutputMaze(maze);



            path = p.BreadthFirstSearch(maze1, startCell, endCell);
            Console.WriteLine("Breadth First");
            Array.Copy(p.UpdateMaze(maze1, path), maze, maze.Length);
            p.OutputMaze(maze);


            //foreach (Point pp in path) { Console.WriteLine(pp); }






            //Console.WriteLine(path1.Count() + " " + path2.Count + " " + path3.Count);
        }

        public List<Point> DijkstraVersion2(int[,] maze, Point start, Point goal)
        {
            UpPriQu Q = new();
            Dictionary<Point, Point> prev = [];
            Point None = new(-1, -1);
            int numExplored = 0;

            Console.WriteLine(maze.GetLength(0));
            Console.WriteLine(maze.GetLength(1));

            for (int row = 0; row < maze.GetLength(0 ); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    Q.Enqueue(point, int.MaxValue);
                    prev.Add(point, None);

                }
            }
            Q.Update(start, 0);
            Point current;
            while (Q.Count > 0)
            {
                numExplored++;
                current = Q.Dequeue();
                if (current == goal)
                {
                    break;
                }

                foreach (Point neighbour in Neighbours(maze, current))
                {
                    if (!Q.Contains(neighbour)) {  continue; }
                    int altDist = Q.Distance(current) + 1;
                    if (altDist < Q.Distance(neighbour))
                    {
                        Q.Update(neighbour, altDist);
                        prev[neighbour] = current;
                    }                    
                }               
            }
            Console.WriteLine(numExplored);
            return RecallPath(prev, goal);
        }

        public List<Point> BreadthFirstSearch(int[,] maze, Point start, Point goal)
        {
            Queue<Point> Q = new();
            Dictionary<Point, Point> cameFrom = [];
            List<Point> visitedCells = [];
            Point None = new(-1, -1);
            visitedCells.Add(start);
            Q.Enqueue(start);
            cameFrom[start] = None;
            Point CurrentCell;
            int numExplored = 0;

            do
            {
                numExplored++;
                CurrentCell = Q.Dequeue();
                if (CurrentCell.Equals(goal))
                {
                    Console.WriteLine("Reached End");
                    break;
                }

                foreach (Point nextCell in Neighbours(maze, CurrentCell))
                {
                    if (!visitedCells.Contains(nextCell))
                    {
                        Q.Enqueue(nextCell);
                        visitedCells.Add(nextCell);
                        cameFrom.Add(nextCell, CurrentCell);
                    }
                }

            }
            while (Q.Count > 0);

            Console.WriteLine(numExplored);
            return RecallPath(cameFrom, goal);

        }

        public List<Point> AStarSearch(int[,]  maze, Point start, Point goal)
        {
            //This rendition of Updatable priority queue has the point then the fscore as thats what have to get minimum from
            UpPriQu Q = new UpPriQu();
            Dictionary<Point, int> gScore = new();
            Dictionary<Point, int> fScore = new();
            Dictionary<Point, Point> prev = new();
            Point None = new(-1, -1);
            int numExplored = 0;

            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new(row, col);
                    gScore.Add(point, int.MaxValue);
                    fScore.Add(point,-1);
                    prev.Add(point, None);                   
                }
            }
            gScore[start] = 0;
            fScore[start] = EuclidianDistance(start, goal);
            Q.Enqueue(start, fScore[start]);

            while (Q.Count > 0)
            {
                numExplored++;
                Point current = Q.Dequeue();
                if (current == goal) { break; }

                foreach(Point neighbour in Neighbours(maze,current))
                {
                    int tentative_gScore = gScore[current] + 1;
                    if(tentative_gScore < gScore[neighbour])
                    {
                        prev[neighbour] = current;
                        gScore[neighbour] = tentative_gScore;
                        fScore[neighbour] = tentative_gScore + EuclidianDistance(current,goal);
                        if (!Q.Contains(neighbour))
                        {
                            Q.Enqueue(neighbour, fScore[neighbour]);
                        }
                    }

                }

            }

            Console.WriteLine(numExplored);
            return RecallPath (prev, goal);
           
        }

        public List<Point> RecallPath(Dictionary<Point, Point> prev, Point goal)
        {
            Point None = new(-1, -1);
            Point current = goal;
            List<Point> path = [];
            path.Clear();

            if (prev[goal] == None)
            {
                Console.WriteLine("No Path Found");
                //when integrated with UI will make this display pop up message
            }

            while (current != None)
            {
                path.Add(current);
                current = prev[current];
            }
            path.Reverse();
            return path;
        }

        public int EuclidianDistance(Point start, Point goal)
        {
            double h = Math.Sqrt(Math.Pow(start.X - goal.X, 2) + Math.Pow(start.Y - goal.Y, 2));
            //return (int)h;
            return (int)Math.Round(h);
        }

        public List<Point> Neighbours(int[,] maze, Point current)
        {
            int[] checksRows = [0, 0, 1, -1];
            int[] checksCols = [1, -1, 0, 0];
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];

            for (int t = 0; t < 4; t++)
            {
                try
                {
                    if (maze[row + checksRows[t], col + checksCols[t]] != 1)
                    {
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch { }
            }
            return neighbours;
        }
        public int[,] RandomMaze(int[,] maze)
        {
            Random random = new();
            for (int i = 0; i < 200; i++)
            {
                int row = random.Next(0, 30);
                int col = random.Next(0, 50);
                if (maze[row, col] == 0)
                {
                    maze[row, col] = 1;
                }
            }
            return maze;
        }

        public void OutputMaze(int[,] maze)
        {
            int tally = 0;
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    if (maze[i, j] == 1) { Console.BackgroundColor = ConsoleColor.Black; }
                    else if (maze[i, j] == 9) { Console.BackgroundColor = ConsoleColor.Blue;tally++; }
                    Console.Write(maze[i, j] + " ");
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(tally);
        }

        public int[,] UpdateMaze(int[,] maze, List<Point> path)
        {
            ////foreach (Point p in path) { Console.WriteLine(p); }
            //Console.WriteLine("Before Update");
            //OutputMaze(maze);
            int[,] tempMaze = new int[30,50];
            Array.Copy(maze, tempMaze, maze.Length);
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if(tempMaze[path[i].X, path[i].Y] == 1) { Console.WriteLine("mucked up here") ; continue; }
                    tempMaze[path[i].X, path[i].Y] = 9;
                }
                catch { Console.WriteLine("coordinates bad"); }
            }
            //Console.WriteLine("After Update");
            //OutputMaze(maze);
            return tempMaze; 
            
        }

        public void RandomizedDFS(int[,] maze, Point vertex, ref List<Point> visited)
        {
            Point none = new Point(-1,-1);
            visited.Add(vertex);
            Point next = randomUnvisitedNeighbour(maze,vertex,ref visited);

            while(next != none)
            {
                ConnectCells(maze, vertex, next);
                Console.Clear();
                OutputMaze(maze);
                RandomizedDFS(maze, next, ref visited);
                next = randomUnvisitedNeighbour(maze,vertex,ref visited);
            }

        }

        public void RandomizedDFSV2(int[,] maze, Point start)
        {
            Queue<Point> queue = new Queue<Point>();
            //Stack<Point> stack = new Stack<Point>();
            List<Point> visited = new();
            Point current = start;
            Point next = new();
            Point none = new(-1,-1);
            queue.Enqueue(current);
            visited.Add(current);

            while (queue.Count > 0) 
            {
                

                next = randomUnvisitedNeighbour(maze, current, ref visited);
                //Console.WriteLine(next);
                if (next == none)
                {
                    while (queue.Count > 0)
                    {
                        next = queue.Dequeue();
                        //Console.WriteLine(next);
                        if (NeighboursMazeGen(maze, next, visited).Count() > 0)
                        {
                            //Console.WriteLine(NeighboursMazeGen(maze, next, visited).Count());
                            current = next;
                            break;
                        }
                    }
                    if (queue.Count == 0)
                    {
                        break;
                    }
                    
                }

                ConnectCells(maze, current, next);
                current = next;
                queue.Enqueue(current);
                visited.Add(current);

                //foreach (Point p in queue) { Console.Write(p); }
                //Console.WriteLine();

            }
        }
        public void RandomizedDFSV3(int[,] maze, Point start)
        {
            Random random = new Random();
            Stack<Point> stack = new Stack<Point>();
            List<Point> visited = new();
            Point current = start;
            Point next = new();
            Point none = new(-1, -1);
            stack.Push(current);
            visited.Add(current);

            while (stack.Count > 0)
            {
                current = stack.Peek();
                List<Point> unvisitedNeighbours = NeighboursMazeGen(maze, current, visited);
                //foreach (Point s in unvisitedNeighbours) { Console.Write(s); }
                //Console.WriteLine();


                if (unvisitedNeighbours.Count > 0)
                {
                    next = unvisitedNeighbours[random.Next(unvisitedNeighbours.Count)]; 
                    ConnectCells(maze, current, next);
                    visited.Add(current);
                    stack.Push(next);

                }
                else { stack.Pop(); }

                Console.Clear();
                OutputMaze(maze);
                

            }
        }

        public int[,] CreateMaze(int[,] maze, Point point)
        {
            Point start = point;
            List<Point> visited = new();
            RandomizedDFS(maze, start, ref visited);
            return maze;

        }

        public void ConnectCells(int[,] maze, Point first, Point second)
        {
            int row = (first.X + second.X) / 2;
            int col = (first.Y + second.Y) / 2;
            maze[row, col] = 0;
        } 
        public Point randomUnvisitedNeighbour(int[,] maze, Point vertex, ref List<Point> visited)
        {
            Random rand = new();
            Point next = new(-1, -1);
            Point none = new(-1,-1);
            List<Point> neighbours = NeighboursMazeGen(maze, vertex,visited);
            while(neighbours.Count > 0)
            {
                next = neighbours[rand.Next(neighbours.Count)];
                neighbours.Remove(next);    
                if (!visited.Contains(next)) {break; }
                next = none;
            }
            return next;

        }
        public List<Point> NeighboursMazeGen(int[,] maze, Point current, List<Point> visited)
        {
            int[] checksRows = [0, 0, 2, -2];
            int[] checksCols = [2, -2, 0, 0];
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = [];

            for (int t = 0; t < 4; t++)
            {
                try
                {
                    if (maze[row + checksRows[t], col + checksCols[t]] != 1 && !visited.Contains(new Point(row + checksRows[t], col + checksCols[t])))
                    {
                        neighbours.Add(new Point(row + checksRows[t], col + checksCols[t]));
                    }

                }
                catch { }
            }
            return neighbours;
        }


        public int[,] GenerateRowsCols(int[,] maze)
        {
            //int r = maze.GetLength(0);
            //int c = maze.GetLength(1);

            for(int r = 0; r < maze.GetLength(0); r++)
            {
                for(int c = 0; c < maze.GetLength(1); c++)
                {
                    if (r%2 == 1) { maze[r, c] = 1; }
                    if(c%2 == 1) { maze[r, c] = 1; }
                }
            }
            return maze;
        }

    }
}
