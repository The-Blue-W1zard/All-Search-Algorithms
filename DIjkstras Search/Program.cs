namespace DIjkstras_Search
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.Versioning;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            int[,] maze = new int[30, 50];
            maze[0, 0] = 2;
            maze[29, 49] = 3;
            Point startCell = new Point(0, 0);
            Point endCell = new Point(29, 49);
            List<Point> path = new List<Point>();
            maze = randomMaze(maze);
            maze[5, 0] = 1;
            maze[6, 1] = 1;
            OutputMaze(maze);

            path = DijkstraVersion2(maze, startCell, endCell);
            foreach (Point p in path ) { Console.WriteLine(p); }
            UpdateMaze(ref maze, path);
            OutputMaze(maze);
        }
        static List<Point> DijkstrasAlgorithm(int[,] maze, Point start, Point end)

        {
            PriorityQueue<Point,int> Q = new PriorityQueue<Point,int>();
            Dictionary<Point, int> dist = new Dictionary<Point, int>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            Point None = new Point(-1, -1);

            for (int row = 0; row < maze.GetLength(0); row++) 
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new Point(row,col);
                    dist.Add(point, int.MaxValue);
                    cameFrom.Add(point, None);
                    Q.Enqueue(point, dist[point]);
                }
            }

            dist[start] = 0;
            Q.Enqueue(start, 0);
            Point currentCell = None;

            while(Q.Count > 0)
            {
                currentCell = Q.Dequeue();

                if (currentCell == end)
                {
                    Console.WriteLine("Reached end");
                    break;
                }
                Console.WriteLine("Current Cell + " +currentCell);


                foreach(Point nextCell in Neighbours(maze, currentCell))
                {
                    if(!Contains(Q, nextCell)) { continue; }

                    int newDistance = dist[currentCell] + 1;
                    Console.WriteLine(nextCell + "," + newDistance + ","+dist[nextCell]);
                    if (newDistance < dist[nextCell])
                    {
                        Console.WriteLine("updating");
                        Q.Enqueue(nextCell, newDistance);
                        dist[nextCell] = newDistance;
                        cameFrom[nextCell] = currentCell;
                       
                    }
                }
                Console.Write("outputting Q:");
                OutputPQ(Q);
                
            } 

            if (cameFrom[end] == None)
            {
                Console.WriteLine("No path found");
            }

            List<Point> path = new List<Point>();
            currentCell = end;
            while (currentCell != None)
            {
                path.Add(currentCell);
                currentCell = cameFrom[currentCell];
            }
            path.Reverse();
            return path;
        }

        static List<Point> DijkstraVersion2(int[,] maze, Point start, Point goal)
        {
            UpPriQu Q = new UpPriQu();
            Dictionary<Point, Point> prev = new Dictionary<Point, Point>();
            Point None = new Point(-1, -1);

            Console.WriteLine(maze.GetLength(0));
            Console.WriteLine(maze.GetLength(1));

            for (int row = 0; row < maze.GetLength(0 ); row++)
            {
                for (int col = 0; col < maze.GetLength(1); col++)
                {
                    Point point = new Point(row, col);
                    Q.Enqueue(point, int.MaxValue);
                    prev.Add(point, None);

                }
            }
            Q.Update(start, 0);
            Point current = None;
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
            if (prev[goal] == None)
            {
                Console.WriteLine("No path found");
            }

            List<Point> path = new List<Point>();
            current = goal;
            while (current != None)
            {
                path.Add(current);
                current = prev[current];
            }
            path.Reverse();
            return path;
        }
        static bool Contains(PriorityQueue<Point, int> q, Point a )
        {
            for (int i = 0; i < q.Count; i++)
            {
                Point test = q.Dequeue();
                if (test.X == a.X && test.Y == a.Y)
                {
                    return true;
                }
            }
            return false;

        }
        static void OutputPQ (PriorityQueue<Point,int> q)
        {
            for(int i = 0; i < q.Count; i++)
            {
                Point test = q.Dequeue();
                Console.Write(test);
            }
            Console.WriteLine(" " + q.Count);

        }
      

        
        static List<Point> Neighbours(int[,] maze, Point current)
        {
            int[] checksRows = { 0, 0, 1, -1 };
            int[] checksCols = { 1, -1, 0, 0 };
            int row = current.X;
            int col = current.Y;
            List<Point> neighbours = new List<Point>();

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
        static int[,] randomMaze(int[,] maze)
        {
            Random random = new Random();
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

        static void UpdateMaze(ref int[,] maze, List<Point> path)
        {
            try
            {
                for (int i = 0; i < path.Count(); i++)
                {
                    maze[path[i].Y, path[i].X] = 9;
                }
            }
            catch { }
        }

    }
}
