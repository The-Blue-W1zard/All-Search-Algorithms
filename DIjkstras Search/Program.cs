namespace DIjkstras_Search
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Versioning;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            int[,] maze1 = new int[30, 50];
            int[,] maze2 = new int[30, 50];
            int[,] maze3 = new int[30, 50];


            maze1[0, 0] = 2;
            maze1[29, 49] = 3;
            maze2[0, 0] = 2;
            maze2[29, 49] = 3;
            Point startCell = new(0, 0);
            Point endCell = new(29, 49);
            maze1 = RandomMaze(maze1);
            maze2 = maze1;
            maze3 = maze1;
            OutputMaze(maze1);

            List<Point> path3 = AStarSearch(maze3, startCell, endCell);
            maze3 = UpdateMaze(maze3, path3);
            Console.WriteLine("A Star");
            OutputMaze(maze3);
            foreach (Point p in path3) { Console.WriteLine(p); }

            List<Point> path1 = DijkstraVersion2(maze1, startCell, endCell);
            maze1 = UpdateMaze(maze1, path1);
             Console.WriteLine("Dijkstras");
            OutputMaze(maze1);


            foreach (Point p in path1) { Console.WriteLine(p); }
            List<Point> path2 = BreadthFirstSearch(maze2, startCell, endCell);
            maze2 = UpdateMaze(maze2, path2);
            Console.WriteLine("Breadth First");
            OutputMaze(maze2);


            

            
            //Console.WriteLine(path1.Count() + " " + path2.Count + " " + path3.Count);
        }

        static List<Point> DijkstraVersion2(int[,] maze, Point start, Point goal)
        {
            UpPriQu Q = new();
            Dictionary<Point, Point> prev = [];
            Point None = new(-1, -1);

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
            
            return RecallPath(prev, goal);
        }

        static List<Point> BreadthFirstSearch(int[,] maze, Point start, Point goal)
        {
            Queue<Point> Q = new();
            Dictionary<Point, Point> cameFrom = [];
            List<Point> visitedCells = [];
            Point None = new(-1, -1);
            visitedCells.Add(start);
            Q.Enqueue(start);
            cameFrom[start] = None;
            Point CurrentCell;

            do
            {
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

            return RecallPath(cameFrom, goal);

        }

        static List<Point> AStarSearch(int[,]  maze, Point start, Point goal)
        {
            //This rendition of Updatable priority queue has the point then the fscore as thats what have to get minimum from
            UpPriQu Q = new UpPriQu();
            Dictionary<Point, int> gScore = new();
            Dictionary<Point, int> fScore = new();
            Dictionary<Point, Point> prev = new();
            Point None = new(-1, -1);

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
            
            return RecallPath (prev, goal);
           
        }

        static List<Point> RecallPath(Dictionary<Point, Point> prev, Point goal)
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

        static int EuclidianDistance(Point start, Point goal)
        {
            double h = Math.Sqrt(Math.Pow(start.X - goal.X, 2) + Math.Pow(start.Y - goal.Y, 2));
            return (int)h;
        }

        static List<Point> Neighbours(int[,] maze, Point current)
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
        static int[,] RandomMaze(int[,] maze)
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

        static void OutputMaze(int[,] maze)
        {
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    if (maze[i, j] == 1) { Console.BackgroundColor = ConsoleColor.Black; }
                    else if (maze[i, j] == 9) { Console.BackgroundColor = ConsoleColor.Blue; }
                    Console.Write(maze[i, j] + " ");
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static int[,] UpdateMaze(int[,] maze, List<Point> path)
        {
            
            for (int i = 0; i < path.Count; i++)
            {
                try
                {
                    if(maze[path[i].X, path[i].Y] == 1) { Console.WriteLine("mucked up here") ; continue; }
                    maze[path[i].X, path[i].Y] = 9;
                }
                catch { Console.WriteLine("coordinates bad"); }
            }
            return maze; 
            
        }

    }
}
